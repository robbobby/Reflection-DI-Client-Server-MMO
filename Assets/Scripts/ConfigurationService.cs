using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public class ConfigurationService {
    public ServiceProvider ServiceProvider { get; private set; }

    public static ConfigurationService CreateInstance() {
        return CreateInstance(s =>  CreateInstance());
    }

    public static ConfigurationService CreateInstance(Action<IServiceCollection> handler) {
        var instance = new ConfigurationService();
        IServiceCollection descriptors = CreateDefaultServiceDescriptors();
        handler(descriptors);

        instance.ServiceProvider = descriptors.BuildServiceProvider();
        return instance;
    }

    private static IServiceCollection CreateDefaultServiceDescriptors() {
        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging(s => s.AddConsole());
        return serviceCollection;
    }
}