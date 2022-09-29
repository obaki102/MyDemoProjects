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
            try
            {
                var storedClaimsIdentityInCache = await _lazyCache.GetAsync<string>("Claimsidentity");
                if (storedClaimsIdentityInCache is not null)
                {
                    var buffer = Convert.FromBase64String(storedClaimsIdentityInCache);
                    using (var deserializationStream = new MemoryStream(buffer))
                    {
                        var identity = new ClaimsIdentity(new BinaryReader(deserializationStream, Encoding.UTF8));
                        principal = new(identity);
                    }
                }
            }
            catch (Exception e)
            {
               
            }
            return new AuthenticationState(principal);
        }

        public void NotifyAuthenticationStateChanged()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
