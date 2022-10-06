using MyDemoProjects.Application.Features.Authentication.Commands;
using MyDemoProjects.Application.Shared.DTOs;
using MyDemoProjects.Application.Shared.DTOs.Request;
using MyDemoProjects.Application.Shared.Models.Request;
using MyDemoProjects.Application.Shared.Models.Response;

namespace MyDemoProjects.UI.Services.Authentication
{
    public interface IAuthentication
    {
        Task<ApplicationResponse<bool>> LoginAsync(LoginUser loginUser);
        Task<ApplicationResponse<bool>> ExternalLoginAsync(LoginExternalUserRequset externalLoginUser);
        Task<ApplicationResponse<bool>> CreateAccountAsync(CreateAccount newUser);
        Task<GoogleAuth2Config> GetGoogleExternalAuthConfig();
    }
}
