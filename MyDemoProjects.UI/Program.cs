using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.SignalR;
using MudBlazor.Services;
using MyDemoProjects.Application;
using MyDemoProjects.Application.Infastructure.Services.Identity;
using MyDemoProjects.Application.Shared.Constants;
using MyDemoProjects.UI.Data;
using MyDemoProjects.UI.Hubs;
using MyDemoProjects.UI.Services.AnimeList;
using MyDemoProjects.UI.Services.Authentication;
using MyDemoProjects.UI.Services.RandomGOTQuotes;
using MyDemoProjects.UI.Services.Shared.Utility.Implementation;
using MyDemoProjects.UI.Services.Shared.Utility.Interface;

var builder = WebApplication.CreateBuilder(args);

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

// Add services to the container.
builder.Services.AddUIApplicationDependencies(builder.Configuration);

builder.Services.AddSingleton<WeatherForecastService>();

//Services
builder.Services.AddScoped<IAnimeList,AnimeList>();
builder.Services.AddScoped<IRandomGOTQuotes, RandomGOTQuotes>();
builder.Services.AddScoped<IAuthentication, Authentication>();
builder.Services.AddScoped<IRetrieveAuthState, RetrieveAuthState>();
builder.Services.AddSingleton<IOnlineUsers, OnlineUsers>();
builder.Services.AddSingleton<IUserIdProvider, EmailBasedUserIdProvider>();
builder.Services.AddScoped<CircuitHandler, UserCircuitHandler>();

//UI
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().AddCircuitOptions(option => { option.DetailedErrors = true; });
builder.Services.AddSignalR().AddMessagePackProtocol();
builder.Services.AddMudServices();

//Must placed after AddServerSideBlazor()
builder.Services.AddScoped<CustomAuthStateProvider>()
               .AddScoped<AuthenticationStateProvider>(provider => provider.GetService<CustomAuthStateProvider>());
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseInfrastructure();
app.UseEndpoints(endpoints =>
{
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
    endpoints.MapHub<ChatHub>(HubConstants.ChatHubUrl);
});
//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;

//    try
//    {
//        var context = services.GetRequiredService<ApplicationDbContext>();

//        if (context.Database.IsSqlServer())
//        {
//            context.Database.Migrate();
//        }
//    }
//    catch (Exception ex)
//    {
//        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

//        logger.LogError(ex, "An error occurred while migrating or seeding the database.");

//        throw;
//    }
//}
await app.RunAsync();