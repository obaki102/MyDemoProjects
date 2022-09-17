global using MyDemoProjects.Shared.DTO.Request;
global using MyDemoProjects.Shared.DTO.Response;
global using MyDemoProjects.Shared.DTO;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyDemoProjects.Client;
using MudBlazor.Services;
using MyDemoProjects.Client.Services.Authentication;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<IAuthentication,Authentication>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();

await builder.Build().RunAsync();
