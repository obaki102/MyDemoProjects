using Microsoft.AspNetCore.Components.Authorization;
using MyDemoProjects.Application.Shared.Constants;
using MyDemoProjects.Application.Shared.Models;
using System.Security.Claims;

namespace MyDemoProjects.UI.Extensions
{
    public static class AuthenticationStateExtension
    {
        public static User GetAuthenticatedUser(this AuthenticationState authState)
        {
            if (authState is null || authState.User is null || authState.User.Identity is null)
            {
                return new();
            }
            var user = new User();  

            foreach (var claims in authState.User.Claims)
            {
                switch (claims.Type)
                {
                    case ApplicationClaimTypes.Status:
                        user.Status = authState.User.Identity.IsAuthenticated;
                        break;
                    case ClaimTypes.NameIdentifier:
                        user.NameIdentifier = claims.Value;
                        break;
                    case ClaimTypes.Name:
                        user.Name = claims.Value;
                        break;
                    case ClaimTypes.Email:
                        user.Email = claims.Value;
                        break;
                    case ApplicationClaimTypes.ProfilePictureDataUrl:
                        user.ProfileUrl = claims.Value;
                        break;
                }
            }
            return user;
        }
    }
}
