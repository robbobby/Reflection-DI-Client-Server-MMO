using System;
using BuggyNet.PackageParser;
using MasterServer.Business;
using MasterServer.Service;
using Microsoft.Extensions.DependencyInjection;

namespace MasterServer {
    internal class Program {
        public static void Main(string[] args) {
            var configurationService = ConfigurationService.CreateInstance();
            configurationService.Provider.GetRequiredService<NetworkService>().Start();
            configurationService.Provider.GetRequiredService<IPackageDispatcher>().Start();
            Console.ReadLine();
        }
    }
}
