using System;
using System.IO;
using BuggyNet.Network.PackageParser;
using MasterServer.Business;
using MasterServer.Service;
using Microsoft.Extensions.DependencyInjection;
using NLog;

namespace MasterServer {
    internal class Program {
        public static void Main(string[] args) {
            var configurationService = ConfigurationService.CreateInstance(serviceDescriptors => {
                serviceDescriptors.AddSingleton<IPackageParser, PackageParser>();
                serviceDescriptors.AddSingleton<IPackageDispatcher, PackageDispatcher>();
                
                serviceDescriptors.AddScoped<ServerConnectionHandler>();
                serviceDescriptors.AddSingleton<NetworkService>();
            
                serviceDescriptors.AddScoped<IUserRepository, UserRepository>();
            });
            configurationService.Provider.GetRequiredService<NetworkService>().Start();
            configurationService.Provider.GetRequiredService<IPackageDispatcher>().Start();
            Console.ReadLine();
        }
    }
}
