using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyDemoProjects.Application.Infastructure.Identity.Extensions
{
    /// <summary>
    /// Try to configure Google account login if the application configuration has ClientId and ClientSecret.
    /// </summary>
    public static class ExternalAuthenticationProviderExtensions
    {
        public static AuthenticationBuilder TryConfigureGoogleAccount(this AuthenticationBuilder authenticationBuilder, IConfiguration configuration)
        {
            var googleAccountClientId = configuration.GetSection(AppSecrets.GoogleClientId).Value;
            var googleAccountClientSecret = configuration.GetSection(AppSecrets.GoogleClientSecret).Value;
            if (string.IsNullOrWhiteSpace(googleAccountClientId) || string.IsNullOrWhiteSpace(googleAccountClientSecret))
                return authenticationBuilder;

            return authenticationBuilder.AddGoogle(options =>
            {
                options.ClientId = googleAccountClientId;
                options.ClientSecret = googleAccountClientSecret;
                //options.Events = new Microsoft.AspNetCore.Authentication.OAuth.OAuthEvents
                //{
                //    OnCreatingTicket = c =>
                //    {
                //        var identity = (ClaimsIdentity?)c?.Principal?.Identity;
                //        var avatar = c?.User.GetProperty("picture").GetString();
                //        if (!string.IsNullOrEmpty(avatar))
                //        {
                //            identity?.AddClaim(new Claim("avatar", avatar));
                //        }
                //        return Task.CompletedTask;
                //    }
                //};
            });
        }
    }
}
