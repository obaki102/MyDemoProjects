using MyDemoProjects.Application.Shared.DTOs.Request;
using MyDemoProjects.Application.Shared.Models.Request;
using MyDemoProjects.Application.Shared.Models.Response;
using MyDemoProjects.Application.Shared.Models.Security;

namespace MyDemoProjects.UI.Services.Authentication
{
    public interface IAuthentication
    {
        Task<ApplicationResponse<bool>> LoginAsync(LoginUserRequest loginUser);
        Task<ApplicationResponse<bool>> ExternalLoginAsync(LoginExternalUserRequset externalLoginUser);
        Task<ApplicationResponse<bool>> CreateAccountAsync(CreateAccountRequest newUser);
    }
}
