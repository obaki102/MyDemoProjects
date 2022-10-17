using Microsoft.AspNetCore.Components.Server.Circuits;
using MyDemoProjects.UI.Services.Authentication;
using MyDemoProjects.UI.Services.Shared.Utility.Interface;

namespace MyDemoProjects.Application.Infastructure.Services.Identity;

public class UserCircuitHandler : CircuitHandler
{
    private readonly CustomAuthStateProvider _customAuthStateProvider;
    public UserCircuitHandler(CustomAuthStateProvider customAuthStateProvider)
    {
        _customAuthStateProvider = customAuthStateProvider;
    }

    public override async Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        await _customAuthStateProvider.GetAuthenticationStateAsync();
       Console.WriteLine($"circuit id: {circuit.Id} User: {_customAuthStateProvider.Name} - connected");
       
    }

    public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        Console.WriteLine($"circuit id: {circuit.Id}  - disconnected");
        return Task.CompletedTask;
    }

}
