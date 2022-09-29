using LazyCache;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MyDemoProjects.Application.Shared.Models.Request;
using MyDemoProjects.Application.Shared.Models.Response;
using System.Text;

namespace MyDemoProjects.Application.Features.Authentication.Commands;

public record LoginUser(LoginUserRequest User) : IRequest<ApplicationResponse<bool>>;

public class LoginUserHandler : IRequestHandler<LoginUser, ApplicationResponse<bool>>
{
    private readonly IIdentityService _identityService;
    private readonly CustomAuthenticationStateProvider _customAuthenticationStateProvider;
    private readonly IAppCache _lazyCache = new CachingService();
    public LoginUserHandler(IIdentityService identityService, CustomAuthenticationStateProvider customAuthenticationStateProvider, IAppCache lazyCache)  
    {
        _identityService = identityService;
        _customAuthenticationStateProvider = customAuthenticationStateProvider;
        _lazyCache = lazyCache;
    }
    public async Task<ApplicationResponse<bool>> Handle(LoginUser request, CancellationToken cancellationToken)
    {
        var isLoginSuccess = await _identityService.LoginUserAsync(request.User.Email, request.User.Password);
        if (isLoginSuccess.IsSuccess is false)
        {
            return ApplicationResponse<bool>.Fail(isLoginSuccess.Messages);
        }
        var identityCreatedFromUser = await _identityService.GenerateClaimsIdentityFromUser(isLoginSuccess.Data);
            //store claims to browser
            using (var memoryStream = new MemoryStream())
            await using (var binaryWriter = new BinaryWriter(memoryStream, Encoding.UTF8, true))
            {
              identityCreatedFromUser.WriteTo(binaryWriter);
              var base64 = Convert.ToBase64String(memoryStream.ToArray());
             _lazyCache.Add("Claimsidentity",  base64, DateTimeOffset.Now.AddMinutes(5));
            }
            _customAuthenticationStateProvider.NotifyAuthenticationStateChanged();

        return ApplicationResponse<bool>.Success("Succesfully login.");
    }
}


