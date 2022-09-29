
using Microsoft.AspNetCore.Builder;
using System.Reflection;


namespace MyDemoProjects.Application;

public static class DIExtension
{
    public static IServiceCollection AddApplicationDependency(this IServiceCollection services, IConfiguration configuration, string baseUrl)
    {
      
        services.AddSignalR();
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddSingleton<IJsonStreamSerializer, JsonStreamSerializer>();
        services.AddScoped<CircuitHandler, UserCircuitHandler>();
        services.AddHttpClient<IHttpService, HttpService>(client =>
        {
            client.BaseAddress = new Uri(baseUrl);
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

        services
                .AddDefaultIdentity<ApplicationUser>()
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

        services.AddScoped<CustomAuthenticationStateProvider>()
                .AddScoped<AuthenticationStateProvider>(provier => provier.GetService<CustomAuthenticationStateProvider>())
                .AddTransient<IIdentityService, IdentyService>();
        return services;
    }




    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseEndpoints(endpoints =>
        {

            endpoints.MapHub<ChatRoomHub>(ChatRoomHub.HubUrl);
        });

        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
}