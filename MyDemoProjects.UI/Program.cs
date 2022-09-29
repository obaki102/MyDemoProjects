using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using MudBlazor.Services;
using MyDemoProjects.Application;
using MyDemoProjects.UI.Data;
using MyDemoProjects.UI.Services.AnimeList;
using MyDemoProjects.UI.Services.Authentication;

var builder = WebApplication.CreateBuilder(args);

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

// Add services to the container.

builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddMudServices();
builder.Services.AddScoped<IAnimeList,AnimeList>();
builder.Services.AddScoped<IAuthentication, Authentication>();
builder.Services.AddUIApplicationDependencies(builder.Configuration, "https://localhost:7205");
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

app.UseInfrastructure(builder.Configuration);
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
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