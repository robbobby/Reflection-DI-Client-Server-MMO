using System;
using BuggyNet.PackageParser;
using DefaultNamespace;
using Microsoft.Extensions.DependencyInjection;

public static class GameContext {
    public static ConfigurationService ConfigService { get; set; }

    static GameContext() {
        ConfigService = ConfigurationService.CreateInstance(serverConfiguration => {
            serverConfiguration.AddSingleton<IPackageParser, PackageParser>();
            serverConfiguration.AddSingleton<ClientConnection>();
        });
    }

    public static ConfigurationService ConfigurationService { get; }
    public static IServiceProvider ServiceProvider => ConfigService.ServiceProvider;
}