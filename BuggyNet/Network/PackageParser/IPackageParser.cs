using System.IO;

namespace BuggyNet.Network.PackageParser {
    public interface IPackageParser {
        Package ParsePackageFromStream(BinaryReader reader);
        void ParsePackageToStream(Package package, BinaryWriter writer);
    }
}