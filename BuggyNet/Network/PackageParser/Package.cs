using System;
using System.IO;
using BuggyNet.Network.Packages;

namespace BuggyNet.Network.PackageParser {
    public abstract class Package {
        public PackageIds PackageId { get; }
        
        protected Package(PackageIds packageId) => this.PackageId = packageId;
        public abstract void DeserialiseFromStream(BinaryReader reader);
        public abstract void SerialiseToStream(BinaryWriter writer);


        
    }
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class PackageRpc : Attribute {
        public PackageIds PackageId { get; }
            
        public PackageRpc(PackageIds packageId) => this.PackageId = packageId;
    }
}