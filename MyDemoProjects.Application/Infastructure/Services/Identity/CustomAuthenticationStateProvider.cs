using LazyCache;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;
using System.Text;


namespace MyDemoProjects.Application.Infastructure.Services.Identity
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IAppCache _lazyCache = new CachingService();
        public CustomAuthenticationStateProvider(IAppCache lazyCache)
        {
            _lazyCache = lazyCache;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var principal = new ClaimsPrincipal(new ClaimsIdentity());
            var storedClaimsIdentityInCache = await _lazyCache.GetAsync<ClaimsIdentity>("Claimsidentity");
            if (storedClaimsIdentityInCache is not null)
            {
                principal = new(storedClaimsIdentityInCache);
            }
            return new AuthenticationState(principal);
        }

        public void NotifyAuthenticationStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
