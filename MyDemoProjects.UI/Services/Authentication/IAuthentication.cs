using MyDemoProjects.Application.Shared.DTOs.Request;
using MyDemoProjects.Application.Shared.DTOs.Response;

namespace MyDemoProjects.UI.Services.Authentication
{
    public interface IAuthentication
    {
        Task<ApplicationResponse<bool>> LoginAsync(LoginUserRequest loginUser);

        Task<ApplicationResponse<bool>> CreateAccountAsync(CreateAccountRequest newUser);
    }
}
