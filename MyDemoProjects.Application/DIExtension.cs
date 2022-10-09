
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using MyDemoProjects.Application.Infastructure.Identity.Extensions;
using System.Reflection;
using MyDemoProjects.Application.Behaviours.Validation;
using MyDemoProjects.Application.Middlewares;
using MyDemoProjects.Application.Infastructure.Identity.Services;
using MyDemoProjects.Application.Features.Shared.Service.Http.AnimeList;
using MyDemoProjects.Application.Features.Shared.Service.Http.RandomGOTQuotes;

namespace MyDemoProjects.Application;

public static class DIExtension
{
    public static IServiceCollection AddUIApplicationDependencies(this IServiceCollection services, IConfiguration configuration, string baseUrl)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        //UI
        services.AddRazorPages();
        services.AddServerSideBlazor().AddCircuitOptions(option => { option.DetailedErrors = true; });
        services.AddSignalR();
        //3rd Party
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        //Middleware
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient<ExceptionHandlingMiddleware>();
        //Services
        services.AddSingleton<IJsonStreamSerializer, JsonStreamSerializer>();
        services.AddHttpClient<IAnimeListHttpService, AnimeListHttpService>();
        services.AddHttpClient<IRandomGotQuotesHttpService, RandomGotQuotesHttpService>();
        //DB
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetSection(AppSecrets.DefaultConnectionString).Value,
                builder =>
                    {
                        builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                        builder.EnableRetryOnFailure(maxRetryCount: 5,
                                                     maxRetryDelay: TimeSpan.FromSeconds(5),
                                                     errorNumbersToAdd: null);
                        builder.CommandTimeout(15);
                    });
        });
        //Utility
        services.AddLazyCache();
        //Authentication
        services
                .AddDefaultIdentity<ApplicationUser>()
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
        services.AddTransient<IIdentityService, IdentityService>();
        //services.AddAuthentication().TryConfigureGoogleAccount(configuration); 

        return services;
    }

    public static IServiceCollection AddAPiApplicationDependencies(this IServiceCollection services, IConfiguration configuration, string baseUrl)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        //3rd Party
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        //Middleware
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddTransient<ExceptionHandlingMiddleware>();
        //Services
        services.AddSingleton<IJsonStreamSerializer, JsonStreamSerializer>();
        services.AddHttpClient<IAnimeListHttpService, AnimeListHttpService>();
        services.AddHttpClient<IRandomGotQuotesHttpService, RandomGotQuotesHttpService>();

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetSection(AppSecrets.DefaultConnectionString).Value,
                builder =>
                {
                    builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    builder.EnableRetryOnFailure(maxRetryCount: 5,
                                             maxRetryDelay: TimeSpan.FromSeconds(5),
                                             errorNumbersToAdd: null);
                    builder.CommandTimeout(15);
                });
        });
        //Utility
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
                        .GetBytes(configuration.GetSection(AppSecrets.TokenKey).Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        return services;
    }




    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        if (app == null)
        {
            throw new ArgumentNullException(nameof(app));
        }
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
}