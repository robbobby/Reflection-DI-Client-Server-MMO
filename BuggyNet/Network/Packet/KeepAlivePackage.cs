using System.IO;
using BuggyNet.Network.PackageParser;

namespace BuggyNet.Network.Packages {
    [PackageRpc(PackageIds.KeepAlive)]
    public class KeepAlivePackage : Package {

        public KeepAlivePackage() : base(PackageIds.KeepAlive) {
            
        }
        public override void SerialiseToStream(BinaryWriter writer) {
            writer.Write((uint)PackageId);
        }
        public override void DeserialiseFromStream(BinaryReader reader) {
            throw new System.NotImplementedException();
        }
    }
}
