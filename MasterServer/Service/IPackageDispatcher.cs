using BuggyNet.Network.PackageParser;
using MasterServer.Service.ServiceModels;

namespace MasterServer.Service {
    public interface IPackageDispatcher {
        bool IsRunning { get; }
        void Start();
        void DispatchPackage(ClientConnection connection, Package package);
    }
}
