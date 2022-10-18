using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;
using MyDemoProjects.Application.Shared.Models;

namespace MyDemoProjects.UI.Services.Authentication.Implementation;

public class UserCircuitHandler : CircuitHandler
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    public UserCircuitHandler(AuthenticationStateProvider authenticationStateProvider)
    {
        _authenticationStateProvider = authenticationStateProvider;
    }

    public override async Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
    {
       var user =  await _authenticationStateProvider.GetAuthenticationStateAsync();

    }

    public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

}
