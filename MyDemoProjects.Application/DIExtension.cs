
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;

namespace MyDemoProjects.Application;

public static class DIExtension
{
    public static IServiceCollection AddUIApplicationDependencies(this IServiceCollection services, IConfiguration configuration, string baseUrl)
    {
        if (services == null)
        {
            throw new ArgumentNullException("services");
        }

        services.AddRazorPages();
        services.AddServerSideBlazor();
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
        services.AddLazyCache();
        //Authentication
        services
                .AddDefaultIdentity<ApplicationUser>()
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        services.AddTransient<IIdentityService, IdentityService>();
        return services;
    }

    public static IServiceCollection AddAPiApplicationDependencies(this IServiceCollection services, IConfiguration configuration, string baseUrl)
    {
        if (services == null)
        {
            throw new ArgumentNullException("services");
        }

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
        services.AddLazyCache();
        //Authentication
        services
                .AddDefaultIdentity<ApplicationUser>()
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


        services.AddTransient<IIdentityService, IdentityService>();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(System.Text.Encoding.UTF8
                        .GetBytes(configuration.GetSection("token_key").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        return services;
    }




    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app, IConfiguration configuration)
    {
        if (app == null)
        {
            throw new ArgumentNullException("builder");
        }
        app.UseEndpoints(endpoints =>
        {

            endpoints.MapHub<ChatRoomHub>(ChatRoomHub.HubUrl);
        });

        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
}