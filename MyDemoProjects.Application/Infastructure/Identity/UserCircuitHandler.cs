using Microsoft.AspNetCore.Components.Server.Circuits;

namespace MyDemoProjects.Application.Infastructure.Identity;

public class UserCircuitHandler : CircuitHandler
{

    public override async Task OnConnectionUpAsync(Circuit circuit, CancellationToken cancellationToken)
    {

        Console.WriteLine($"circuit id: {circuit.Id}");
    }

    public override Task OnConnectionDownAsync(Circuit circuit, CancellationToken cancellationToken)
    {

        return Task.CompletedTask;
    }

}
