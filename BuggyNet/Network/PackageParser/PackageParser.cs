using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BuggyNet.Network.Packages;
using Microsoft.Extensions.Logging;

namespace BuggyNet.Network.PackageParser {
    public class PackageParser : IPackageParser {

        private readonly ILogger<PackageParser> logger;
        private Dictionary<PackageIds, Type> packages = new Dictionary<PackageIds, Type>();
        
        public PackageParser(ILogger<PackageParser> logger) {
            this.logger = logger;
            ResolvePackage();
        }
        private void ResolvePackage()
        {
            IEnumerable<Type> packageClasses = GetType().Assembly.GetTypes().
                Where(x => x.IsSubclassOf(typeof(Package)));
            // ReSharper disable once InconsistentNaming
            foreach (Type _class in packageClasses)
            {
                object[] attribute = _class.GetCustomAttributes(typeof(PackageRpc), false);
                if (attribute.FirstOrDefault() is PackageRpc packageId) {
                    packages.Add(packageId.PackageId, _class);
                }
            }
            logger.LogInformation($"Scanned {packages.Count} packages");
        }

        public Package ParsePackageFromStream(BinaryReader reader)
        {
            var packageId = (PackageIds)reader.ReadUInt32();
            if (!packages.TryGetValue(packageId, out Type type))
                throw new InvalidOperationException("Package is unknown");
            if (Activator.CreateInstance(type) is Package package) {
                package.DeserialiseFromStream(reader);
                logger.LogInformation($"Received package from stream: {package.GetType()}");
                return package;
            }
            throw new InvalidOperationException("Package is unknown");
        }

        public void ParsePackageToStream(Package package, BinaryWriter writer) {
            logger.LogInformation($"Write package to stream type: {package.GetType()}");
            package.SerialiseToStream(writer);
            writer.Flush();
        }
    }
}