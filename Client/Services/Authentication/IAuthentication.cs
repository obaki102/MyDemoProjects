using MyDemoProjects.Shared.DTO.Request;
using MyDemoProjects.Shared.DTO.Response;

namespace MyDemoProjects.Client.Services.Authentication
{
    public interface IAuthentication
    {
        Task<ServerResponse<string>> AuthenticateAsync(LoginUserRequest loginUserRequest);
        Task<ServerResponse<bool>> CreateAccountAsync(UserDto newUser);
    }
}
