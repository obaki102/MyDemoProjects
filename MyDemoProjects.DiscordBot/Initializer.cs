using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using MyDemoProjects.Application.Infastructure.Identity.Extensions;
using MyDemoProjects.Application.Shared.Constants;

namespace MyDemoProjects.DiscordBot
{
    public static class Initializer
    {
        public static IServiceProvider ServiceProvider { get; set; }
        private static IServiceCollection _serviceCollection;
        private static bool _isInitialized = false;


        public static void Init()
        {
            if (!_isInitialized)
            {
                var serviceCollection = new ServiceCollection();
                var serviceProvider = serviceCollection
                    .BuildServiceProvider();

                _serviceCollection = serviceCollection;
                ServiceProvider = serviceProvider;
                _isInitialized = true;
            }
        }

        public static void RegisterSingletonType<TInterface, TImplementation>()
            where TInterface : class
            where TImplementation : class, TInterface
        {
            _serviceCollection.AddSingleton<TInterface, TImplementation>();
        }

        public static void RegisterSingletonInstance<TInterface>(TInterface instance)
            where TInterface : class
        {
            _serviceCollection.AddSingleton(instance);
        }

        public static void RegisterSingletonHubConenction(IConfiguration config)
        {
            _serviceCollection.AddSingletonChatHubClient(options =>
            {
                options.HubUrl = config.GetSection(AppSecrets.SignalR.AzureFunctionHubUrl).Value;
            });
        }

        public static void BuildServiceProvider()
        {
            ServiceProvider = _serviceCollection.BuildServiceProvider();
        }
    }
}
