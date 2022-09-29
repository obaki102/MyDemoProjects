namespace MyDemoProjects.Application.Shared.DTOs.Response;
public record UserDetailsResponse
{
    public string Email { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string UserName { get; init; } = string.Empty;
}

