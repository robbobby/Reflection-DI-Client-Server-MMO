using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using BuggyNet.Network.PackageParser;
using BuggyNet.Network.Packages;
using MasterServer.Service.ServiceModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MasterServer.Service {
    class NetworkService {
        private readonly object locker = new object();
        private List<ClientConnection> clients = new List<ClientConnection>();
        private List<ClientConnection> invalidConnections = new List<ClientConnection>();

        private int receivePackageIterationCounter = 0;
        

        private TcpListener tcpListener;
        private Thread listenerWorkerThread;
        private Thread clientPackageReceiverThread;

        public bool IsRunning { get; private set; }
        public event Action<TcpClient> ClientConnected;

        public ILogger logger;
        private readonly IServiceProvider serviceProvider;
        private readonly IPackageParser packageParser;
        private readonly IPackageDispatcher packageDispatcher;
        
        public NetworkService(IConfigurationRoot config, ILogger<NetworkService> logger, IServiceProvider serviceProvider, 
                IPackageParser packageParser, IPackageDispatcher packageDispatcher) {
            tcpListener = new TcpListener(IPAddress.Parse(config.GetValue<string>("host")), config.GetValue<int>("port"));
            IsRunning = false;
            this.logger = logger;
            this.serviceProvider = serviceProvider;
            this.packageParser = packageParser;
            this.packageDispatcher = packageDispatcher;
        }

        public NetworkService(IPAddress address, int port, ILogger<NetworkService> logger, IServiceProvider serviceProvider) {
            tcpListener = new TcpListener(address, port);
            IsRunning = false;
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }
        
        public void Start() {
            if (listenerWorkerThread != null && listenerWorkerThread.ThreadState == ThreadState.Running) {
                return;
            }
            listenerWorkerThread = new Thread(async () => {
                IsRunning = true;
                tcpListener.Start();
                logger.LogInformation("Network service successfully started");
                try {
                    while (IsRunning) {
                        await Task.Delay(100);
                        if (IsRunning && tcpListener.Pending())
                            OnClientConnected(await tcpListener.AcceptTcpClientAsync());
                    }
                } finally {
                    logger.LogInformation("Server stopped...");
                }
            }) { IsBackground = true };
            listenerWorkerThread.Start();
            clientPackageReceiverThread = new Thread(ReceivePackage) { IsBackground = true };
            clientPackageReceiverThread.Start();
        }
        
        
        private async void ReceivePackage() {
            try {
                logger.LogInformation("NetworkService.ReceivePackage thread successfully started");
                while (IsRunning) {
                    await Task.Delay(1);
                    if (IsRunning)
                        lock (locker) {
                            if (++receivePackageIterationCounter == 1000) {
                                receivePackageIterationCounter = 0;
                                foreach (var client in clients) {
                                    try {
                                        packageParser.ParsePackageToStream(new KeepAlivePackage(), client.Writer);
                                    }
                                    catch (Exception e) {
                                        invalidConnections.Add(client); // TODO: Set this to use time and keep alive every 5/6 seconds
                                        clients.Remove(client);
                                        logger.LogInformation($"KEEP_ALIVE Exception client: {client.ConnectionId}");
                                    }
                                }
                            }
                            if (invalidConnections.Count > 0) {
                                foreach (var client in invalidConnections) {
                                    clients.Remove(client);
                                }
                                invalidConnections.Clear();
                            }
                        }
                    var clientConnArr = clients.ToArray();
                    foreach (var client in clientConnArr) {
                        if (client.AvailableBytes > 0) {
                            logger.LogInformation($"Package from client {client.ConnectionId}");
                            var package = packageParser.ParsePackageFromStream(client.Reader);
                            packageDispatcher.DispatchPackage(client, package);
                        }

                    }
                }
            }finally {
                logger.LogInformation("Server stopped...");   
            }
        }

        protected void OnClientConnected(TcpClient connection) {
            ClientConnected?.Invoke(connection);
            var client = new ClientConnection(connection, Guid.NewGuid());
            lock (locker) 
                clients.Add(client);
            logger.LogInformation($"New Client connected Guid: {client.ConnectionId}");
        }

        public void Stop() {
            if (listenerWorkerThread == null)
                return;
            IsRunning = false;
            listenerWorkerThread.Abort();
            listenerWorkerThread = null;
            clientPackageReceiverThread.Abort();
            clientPackageReceiverThread = null;
            tcpListener.Stop();
        }
    }
}