using System;
using System.Diagnostics;
using BuggyNet.PackageParser;
using MasterServer.Business;
using MasterServer.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using ILogger = NLog.ILogger;

namespace MasterServer {
    public class ConfigurationService {
        public ServiceProvider Provider { get; private set; }
        public static ConfigurationService CreateInstance() {
            return CreateInstance(s => {});
        }

        public static ConfigurationService CreateInstance(Action<IServiceCollection> handler) {
            ConfigurationService instance = new ConfigurationService();

            IServiceCollection descriptors = CreateDefaultServiceDescriptors();

            handler(descriptors);
            instance.Provider = descriptors.BuildServiceProvider();
            return instance;
        }
        private static IServiceCollection CreateDefaultServiceDescriptors() {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("Config.json").Build();
            IServiceCollection serviceDescriptors = new ServiceCollection();

            serviceDescriptors.AddLogging(configure => configure.AddConsole());
            serviceDescriptors.AddSingleton<IPackageParser, PackageParser>();
            serviceDescriptors.AddSingleton<IPackageDispatcher, PackageDispatcher>();
                
            serviceDescriptors.AddScoped<ServerConnectionHandler>();
            serviceDescriptors.AddSingleton<NetworkService>();
            
            serviceDescriptors.AddSingleton<IUserRepository, UserRepository>();
            serviceDescriptors.AddSingleton<IConfiguration>();
            
                
            serviceDescriptors.AddSingleton<IConfigurationRoot>(configuration);
            return serviceDescriptors;
        }
        
        
    }
}
