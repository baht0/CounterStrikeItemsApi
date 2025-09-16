using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace WebAdminPanel.Services.Auth
{
    public class JwtAuthStateProvider(IJSRuntime js) : AuthenticationStateProvider
    {
        private readonly IJSRuntime _js = js;

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _js.InvokeAsync<string>("localStorage.getItem", "authToken");
            var identity = string.IsNullOrWhiteSpace(token) || token.Contains("null")
                ? new ClaimsIdentity() 
                : new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");

            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }

        public void MarkUserAsAuthenticated(string token)
        {
            var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void MarkUserAsLoggedOut()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity());
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = Convert.FromBase64String(PadBase64(payload));

            var claims = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            return claims?.Select(kvp =>
                new Claim(kvp.Key ?? string.Empty, kvp.Value?.ToString() ?? string.Empty)
            ) ?? [];
        }


        private static string PadBase64(string base64) 
            => base64.PadRight(base64.Length + (4 - base64.Length % 4) % 4, '=');
    }
}
