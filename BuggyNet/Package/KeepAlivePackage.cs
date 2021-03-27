using System.IO;
using BuggyNet.PackageParser;

namespace BuggyNet.Package {
    [PackageRpc(PackageIds.KeepAlive)]
    public class KeepAlivePackage : PackageParser.Package {

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
