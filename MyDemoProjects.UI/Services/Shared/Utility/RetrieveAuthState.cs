using Microsoft.AspNetCore.Components.Authorization;
using MyDemoProjects.Application.Shared.Constants;
using System.Security.Claims;

namespace MyDemoProjects.UI.Services.Shared.Utility;
public class RetrieveAuthState : IRetrieveAuthState
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    public RetrieveAuthState(AuthenticationStateProvider authenticationStateProvider)
    {
        _authenticationStateProvider = authenticationStateProvider;

    }
    public async Task<Dictionary<string, string>> GetClaimValues()
    {
        var claimValues = new Dictionary<string, string>();
        var userState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        if (userState == null)
            return claimValues;

        foreach (var claims in userState.User.Claims)
        {
            switch (claims.Type)
            {
                case ApplicationClaimTypes.Status:
                    claimValues.Add(ApplicationClaimTypes.Status, userState.User.Identity.IsAuthenticated.ToString());
                    break;
                case ClaimTypes.NameIdentifier:
                    claimValues.Add(ClaimTypes.NameIdentifier, claims.Value);
                    break;
                case ClaimTypes.Name:
                    claimValues.Add(ClaimTypes.Name, claims.Value);
                    break;
                case ClaimTypes.Email:
                    claimValues.Add(ClaimTypes.Email, claims.Value);
                    break;
                case ApplicationClaimTypes.ProfilePictureDataUrl:
                    claimValues.Add(ApplicationClaimTypes.ProfilePictureDataUrl, claims.Value);
                    break;
                case ApplicationClaimTypes.Expiration:
                    claimValues.Add(ClaimTypes.Expiration, claims.Value);
                    break;
            }
        }

        return claimValues;
    }
}
