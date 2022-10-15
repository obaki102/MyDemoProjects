using Microsoft.Extensions.DependencyInjection;
using MyDemoProjects.DiscordBot.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static void RegisterType<TInterface, TImplementation>()
            where TInterface : class
            where TImplementation : class, TInterface
        {
            _serviceCollection.AddSingleton<TInterface, TImplementation>();
        }

        public static void RegisterInstance<TInterface>(TInterface instance)
            where TInterface : class
        {
            _serviceCollection.AddSingleton(instance);
           
        }

        public static void BuildServiceProvider()
        {
            ServiceProvider = _serviceCollection
                               .BuildServiceProvider();
        }
    }
}
