using Duende.IdentityServer.Extensions;
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

            var user = new User
            {
                Email = authState.User.FindFirst(f => f.Type == ClaimTypes.Email).Value,
                Name = authState.User.FindFirst(f => f.Type == ClaimTypes.Name).Value,
                NameIdentifier = authState.User.FindFirst(f => f.Type == ClaimTypes.NameIdentifier).Value,
                ProfileUrl = authState.User.FindFirst(f => f.Type == ApplicationClaimTypes.ProfilePictureDataUrl).Value,
                Status = authState.User.Identity.IsAuthenticated

            };

            return user;
        }
    }
}
