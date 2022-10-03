using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace MyDemoProjects.UI.Services.Authentication
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ProtectedLocalStorage _protectedLocalStorage;
        private readonly HttpClient _httpClient;

        public CustomAuthStateProvider(ProtectedLocalStorage protectedLocalStorage, HttpClient httpClient)
        {
            _protectedLocalStorage = protectedLocalStorage;
            _httpClient = httpClient;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {

            var authToken = await _protectedLocalStorage.GetAsync<string>("authToken");

            var identity = new ClaimsIdentity();
            _httpClient.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(authToken.Value))
            {
                try
                {
                    identity = new ClaimsIdentity(ParseClaimsFromJwt(authToken.Value), "jwt");
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", authToken.Value.Replace("\"", ""));
                }
                catch
                {
                    await _protectedLocalStorage.DeleteAsync("authToken");
                    identity = new ClaimsIdentity();
                }
            }

            var user = new ClaimsPrincipal(identity);
            var state = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;
        }
        public async Task LogOutAndUpdateAuthenticationState()
        {
            await _protectedLocalStorage.DeleteAsync("authToken");
            await GetAuthenticationStateAsync();
        }

        public async Task SaveJwtToLocalStorageAndUpdateAuthenticationState(string jwt)
        {
            await _protectedLocalStorage.SetAsync("authToken", jwt);
            await GetAuthenticationStateAsync();

        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer
                .Deserialize<Dictionary<string, object>>(jsonBytes);

            var claims = keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));

            return claims;
        }
    }
}
