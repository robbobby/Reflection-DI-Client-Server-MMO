using System;
using BuggyNet.PackageParser;
using MasterServer.Business;
using MasterServer.Models.User;
using MasterServer.Service;
using Microsoft.Extensions.DependencyInjection;

namespace MasterServer {
    internal static class Program {
        public static void Main(string[] args) {
            var configurationService = ConfigurationService.CreateInstance();
            configurationService.ServiceProvider.GetRequiredService<NetworkService>().Start();
            configurationService.ServiceProvider.GetRequiredService<IPackageDispatcher>().Start();
            UserRepository x = (UserRepository) configurationService.ServiceProvider.GetService(typeof(IUserRepository));
            Console.WriteLine(x.UserExists("Username", "Password"));
            Console.ReadLine();
        }
    }
}
