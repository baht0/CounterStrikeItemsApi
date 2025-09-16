using Microsoft.JSInterop;
using MudBlazor;
using System.IdentityModel.Tokens.Jwt;

namespace WebAdminPanel
{
    public partial class App
    {
        private MudThemeProvider? _mudThemeProvider;

        protected override void OnInitialized()
        {
            ThemeService.OnThemeChanged += StateHasChanged;
        }

        public void Dispose() => ThemeService.OnThemeChanged -= StateHasChanged;

        private async Task HandleNavigation(Microsoft.AspNetCore.Components.Routing.NavigationContext context)
        {
            var currentUrl = context.Path.Trim('/').ToLower();
            var publicPages = new[] { "login" };
            if (!publicPages.Contains(currentUrl))
            {
                if (!await IsTokenValidAsync())
                {
                    await JS.InvokeVoidAsync("localStorage.removeItem", "authToken");
                    Nav.NavigateTo("/login", forceLoad: true);
                }
            }
        }
        private async Task<bool> IsTokenValidAsync()
        {
            var token = await JS.InvokeAsync<string>("localStorage.getItem", "authToken");
            if (string.IsNullOrWhiteSpace(token))
                return false;

            try
            {
                var handler = new JwtSecurityTokenHandler();
                if (!handler.CanReadToken(token))
                    return false;

                var jwtToken = handler.ReadJwtToken(token);

                var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;
                if (string.IsNullOrEmpty(expClaim))
                    return false;

                var expUnix = long.Parse(expClaim);
                var expDate = DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;

                return expDate > DateTime.UtcNow;
            }
            catch
            {
                return false;
            }
        }
    }
}