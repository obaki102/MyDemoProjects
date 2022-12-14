using MyDemoProjects.Application.Features.Authentication.Commands;
using MyDemoProjects.Application.Shared.DTOs;
using MyDemoProjects.Application.Shared.DTOs.Request;
using MyDemoProjects.Application.Shared.Models.Request;
using MyDemoProjects.Application.Shared.Models.Response;

namespace MyDemoProjects.UI.Services.Authentication.Interface
{
    public interface IAuthentication
    {
        Task<ApplicationResponse<bool>> LoginAsync(LoginUser loginUser);
        Task<ApplicationResponse<bool>> ExternalLoginAsync(LoginExternalUser externalLoginUser);
        Task<ApplicationResponse<bool>> CreateAccountAsync(CreateAccount newUser);
        Task<GoogleAuth2Config> GetGoogleExternalAuthConfig();
        Task LogOutAndUpdateAuthenticationState();
        Task SaveJwtToLocalStorageAndUpdateAuthenticationState(string jwt);


    }
}
