
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MyDemoProjects.Application;

public static class DIExtension
{
    public static IServiceCollection AddApplicationDependency(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSignalR();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddSingleton<IJsonStreamSerializer, JsonStreamSerializer>();
        services.AddScoped<CircuitHandler>((sp) => new UserCircuitHandler());
        services.AddHttpClient<IHttpService, HttpService>(client =>
        {
            client.BaseAddress = new Uri("https://localhost:7205/");
        });
        services.AddDbContext<ApplicationDbContext>(options =>
        {
        options.UseSqlServer(configuration.GetSection("DefaultConnection").Value,
            builder =>
                {
                    builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    builder.EnableRetryOnFailure(maxRetryCount: 5,
                                                 maxRetryDelay: TimeSpan.FromSeconds(5),
                                                 errorNumbersToAdd: null);
                    builder.CommandTimeout(15);
                });
        });

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseEndpoints(endpoints =>
        {

            endpoints.MapHub<ChatRoomHub>(ChatRoomHub.HubUrl);
        });

        return app;
    }
}