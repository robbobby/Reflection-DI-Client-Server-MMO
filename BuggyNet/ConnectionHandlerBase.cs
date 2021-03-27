using System;
using System.Reflection;
using BuggyNet.Package;

namespace BuggyNet {
    public abstract class ConnectionHandlerBase<T> {
        
        public void InvokeAction(T connection, object parsedData, PackageIds packageIds) {
            foreach (MethodInfo method in GetType().GetMethods()) {
                PackageHandler attribute = method.GetCustomAttribute<PackageHandler>();
                if(attribute == null) continue;
                if (attribute.PackageId != packageIds) continue;
                method.Invoke(this, new[] {connection, parsedData});
                return;
            }
            HandleUnknownPacket(connection, parsedData, packageIds);
        }

        protected abstract void HandleUnknownPacket(T connection, object parsedData, PackageIds type);
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class PackageHandler : Attribute {
        public PackageIds PackageId { get; }
        public PackageHandler(PackageIds packageId) {
            this.PackageId = packageId;
        }
    }
}