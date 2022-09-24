global using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MyDemoProjects.Client;
using MudBlazor.Services;
using MyDemoProjects.Client.Services.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using MyDemoProjects.Client.Services.AnimeList;
using MyDemoProjects.Client.Services.ChatRoom;
using System.Net;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<IAuthentication,Authentication>();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<IAnimeList, AnimeList>();
builder.Services.AddSingleton<IChatRoom, ChatRoom>();


await builder.Build().RunAsync();


