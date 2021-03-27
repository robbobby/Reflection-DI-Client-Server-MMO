using System.IO;

namespace BuggyNet.PackageParser {
    public interface IPackageParser {
        Package ParsePackageFromStream(BinaryReader reader);
        void ParsePackageToStream(Package package, BinaryWriter writer);
    }
}