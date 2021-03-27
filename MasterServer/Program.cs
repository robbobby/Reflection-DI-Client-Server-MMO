using System;
using BuggyNet.Network.PackageParser;
using MasterServer.Business;
using MasterServer.Service;
using Microsoft.Extensions.DependencyInjection;

namespace MasterServer {
    internal class Program {
        public static void Main(string[] args) {
            var configurationService = ConfigurationService.CreateInstance(serviceDescriptors => {
                serviceDescriptors.AddSingleton<IPackageParser, PackageParser>();
                serviceDescriptors.AddSingleton<IPackageDispatcher, PackageDispatcher>();
                
                serviceDescriptors.AddScoped<IServerConnectionHandler, ServerConnectionHandler>();
                serviceDescriptors.AddSingleton<INetworkService, NetworkService>();

                serviceDescriptors.AddScoped<IUserRepository, UserRepository>();
            });
            configurationService.Provider.GetRequiredService<NetworkService>().Start();
            configurationService.Provider.GetRequiredService<PackageDispatcher>().Start();
            Console.ReadLine();
        }
    }
}
