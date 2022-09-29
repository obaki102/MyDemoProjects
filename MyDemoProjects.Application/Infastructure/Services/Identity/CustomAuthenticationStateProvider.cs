using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Security.Claims;
using System.Text;


namespace MyDemoProjects.Application.Infastructure.Services.Identity
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedLocalStorage _protectedLocalStorage;
        public CustomAuthenticationStateProvider(ProtectedLocalStorage protectedLocalStorage)
        {
            _protectedLocalStorage = protectedLocalStorage;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var principal = new ClaimsPrincipal(new ClaimsIdentity());
            try
            {
                var storedClaimsIdentity = await _protectedLocalStorage.GetAsync<string>("Claimsidentity");
                if (storedClaimsIdentity.Success && storedClaimsIdentity.Value is not null)
                {
                    var buffer = Convert.FromBase64String(storedClaimsIdentity.Value);
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
