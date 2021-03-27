using System;
using System.Collections.Concurrent;
using System.Threading;
using BuggyNet.Network.PackageParser;
using MasterServer.Service.ServiceModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MasterServer.Service {
    public class PackageDispatcher : IPackageDispatcher {
        private readonly ILogger<IPackageDispatcher> logger;
        private readonly IServiceProvider serviceProvider;
        private readonly IPackageParser packageParser;
        
        private BlockingCollection<Tuple<ClientConnection, Package>> queue = new BlockingCollection<Tuple<ClientConnection, Package>>();
        private Thread dispatcherThread;
        
        public bool IsRunning { get; private set; }

        public PackageDispatcher(ILogger<IPackageDispatcher> logger, IServiceProvider serviceProvider, IPackageParser packageParser) {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
            this.packageParser = packageParser;
        }


        public void Start() {
            dispatcherThread = new Thread(() => {
                    logger.LogInformation("Dispatcher started");
                    IsRunning = true;
                    try {
                        while (IsRunning) {
                            if (queue.TryTake(out var item)) {
                                using (var scope = serviceProvider.CreateScope()) {
                                    var (connection, package) = item;
                                    var connectionHandler = scope.ServiceProvider.GetRequiredService<ServerConnectionHandler>();
                                    connectionHandler.InvokeAction(connection, package, package.PackageId);
                                }
                            }
                        }
                    }
                    finally {
                        logger.LogError("Dispatcher stopped...");
                    }
                })
                { IsBackground = true };
            dispatcherThread.Start();
        }
        
        public void DispatchPackage(ClientConnection connection, Package package) {
            queue.Add(new Tuple<ClientConnection, Package>(connection, package));
        }
    }
}
