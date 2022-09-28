using MyDemoProjects.Application.Shared.Models.Request;
using MyDemoProjects.Application.Shared.Models.Response;

namespace MyDemoProjects.UI.Services.Authentication
{
    public interface IAuthentication
    {
        Task<ApplicationResponse<bool>> LoginAsync(LoginUserRequest loginUser);

        Task<ApplicationResponse<bool>> CreateAccountAsync(CreateAccountRequest newUser);
    }
}
