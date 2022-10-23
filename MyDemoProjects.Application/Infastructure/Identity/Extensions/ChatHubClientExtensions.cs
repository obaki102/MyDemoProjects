using Microsoft.Extensions.DependencyInjection.Extensions;
using MyDemoProjects.Application.Infastructure.Hubs;
using MyDemoProjects.Application.Infastructure.Hubs.Implementation;
using MyDemoProjects.Application.Infastructure.Hubs.Interface;


namespace MyDemoProjects.Application.Infastructure.Identity.Extensions
{
    public static class ChatHubClientExtensions
    {
        public static IServiceCollection AddTransientChatHubClient(this IServiceCollection services, Action<HubClientOptions> options)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddTransient<IHubClient,ChatHubClient>();
            services.Configure(options);
            return services;
        }

        public static IServiceCollection AddScopedChatHubClient(this IServiceCollection services, Action<HubClientOptions> options)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.Configure(options);
            services.TryAddScoped<IHubClient, ChatHubClient>();
           
            return services;
        }

        public static IServiceCollection AddSingletonChatHubClient(this IServiceCollection services, Action<HubClientOptions> options)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddSingleton<IHubClient, ChatHubClient>();
            services.Configure(options);
            return services;
        }
        public static IServiceCollection AddSingletonChatHubClient(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.TryAddSingleton<IHubClient, ChatHubClient>();
            return services;
        }
    }
}
