using Microsoft.AspNetCore.Components.Server.Circuits;
using MyDemoProjects.UI.Services.Authentication;
using MyDemoProjects.UI.Services.Shared.Utility.Interface;

namespace MyDemoProjects.Application.Infastructure.Services.Identity;

public class UserCircuitHandler : CircuitHandler
{
    private readonly CustomAuthStateProvider _customAuthStateProvider;
    private readonly IOnlineUsers _onlineUsers;

    public UserCircuitHandler(CustomAuthStateProvider customAuthStateProvider, IOnlineUsers onlineUsers)
    {
        _customAuthStateProvider = customAuthStateProvider;
        _onlineUsers = onlineUsers;
    }

    public override async Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        await _customAuthStateProvider.GetAuthenticationStateAsync();
       Console.WriteLine($"circuit id: {circuit.Id} User: {_customAuthStateProvider.Name} - connected");
        _onlineUsers.AddOrUpdate(_customAuthStateProvider.NameIdentifier, _customAuthStateProvider.UserSettings);
       
    }

    public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        Console.WriteLine($"circuit id: {circuit.Id}  - disconnected");
        _onlineUsers.TryRemove(_customAuthStateProvider.NameIdentifier);
        return Task.CompletedTask;
    }

}
