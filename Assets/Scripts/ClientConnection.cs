using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using UnityEngine;

namespace DefaultNamespace {
    public class ClientConnection : MonoBehaviour {
        private TcpClient client;
        public bool IsConnected => client?.Connected ?? false;
        public BinaryWriter Writer { get; private set; }
        public BinaryReader Reader { get; private set; }

        public ClientConnection() {
            Debug.Log("Client connection created");
        }

        public Task Connect(string host, int port) {
            client = new TcpClient();
            client.Connect(host, port);

            var stream = client.GetStream();
            Writer = new BinaryWriter(stream);
            Reader = new BinaryReader(stream);
            return Task.CompletedTask;
        }

        public Task Disconnect() {
            client.Close();
            Writer = null;
            Reader = null;
            return Task.CompletedTask;
        }
    }
}