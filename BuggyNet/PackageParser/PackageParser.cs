using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BuggyNet.Package;
using Microsoft.Extensions.Logging;

namespace BuggyNet.PackageParser {
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
                Where(x => x.IsSubclassOf(typeof(BuggyNet.PackageParser.Package)));
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

        public BuggyNet.PackageParser.Package ParsePackageFromStream(BinaryReader reader)
        {
            var packageId = (PackageIds)reader.ReadUInt32();
            if (!packages.TryGetValue(packageId, out Type type))
                throw new InvalidOperationException("Package is unknown");
            if (!(Activator.CreateInstance(type) is BuggyNet.PackageParser.Package package))
                throw new InvalidOperationException("Package is unknown");
            package.DeserialiseFromStream(reader);
            logger.LogInformation($"Received package from stream: {package.GetType()}");
            return package;
        }

        public void ParsePackageToStream(BuggyNet.PackageParser.Package package, BinaryWriter writer) {
            logger.LogInformation($"Write package to stream type: {package.GetType()}");
            package.SerialiseToStream(writer);
            writer.Flush();
        }
    }
}