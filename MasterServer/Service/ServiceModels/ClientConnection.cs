using System;
using System.IO;
using System.Net.Sockets;

namespace MasterServer.Service.ServiceModels {
    public class ClientConnection {
        public Guid ConnectionId { get; private set; }

        private TcpClient tcpClient;
        private readonly NetworkStream stream;
        public BinaryReader Reader { get; }
        public BinaryWriter Writer { get; }
        
        public ClientConnection(TcpClient tcpClient, Guid connectionId) {
            this.tcpClient = tcpClient;
            ConnectionId = connectionId;
            stream = tcpClient.GetStream();
            Reader = new BinaryReader(stream);
            Writer = new BinaryWriter(stream);
        }

        public int AvailableBytes => tcpClient.Available;
        public bool IsConnected => tcpClient.Connected;
    }
}
