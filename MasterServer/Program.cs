using System;
using BuggyNet.PackageParser;
using MasterServer.Business;
using MasterServer.Models.User;
using MasterServer.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MasterServer {
    internal static class Program {
        public static string UserDbConnectionString { get; private set; }
        public static void Main(string[] args) {
            var configurationService = ConfigurationService.CreateInstance();
            UserDbConnectionString = configurationService.ServiceProvider.GetService<IConfiguration>().GetConnectionString("RemoteMySQL");
            configurationService.ServiceProvider.GetRequiredService<NetworkService>().Start();
            configurationService.ServiceProvider.GetRequiredService<IPackageDispatcher>().Start();
            Console.ReadLine();
        }
    }
}
