using Microsoft.JSInterop;
using System.Net.Http.Headers;
using WebAdminPanel.Models;
using WebAdminPanel.Models.DTOs.Auth;

namespace WebAdminPanel.Pages
{
    public partial class Login
    {
        private AuthDto authModel = new();
        private string? errorMessage;

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity?.IsAuthenticated == true)
            {
                Nav.NavigateTo("/items", true);
            }
        }

        private async Task HandleLogin()
        {
            try
            {
                var response = await AuthApi.Login(authModel);
                var accessToken = response.GetFirstValue();

                if (accessToken != null)
                {
                    await JS.InvokeVoidAsync("localStorage.setItem", "authToken", accessToken);

                    AuthProvider.MarkUserAsAuthenticated(accessToken);
                    Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    Nav.NavigateTo("/items");
                }
            }
            catch (ApiBadRequestException)
            {
                errorMessage = "Login failed. Please check your credentials.";
            }
            catch (Exception ex)
            {
                errorMessage = "Something went wrong.";
                Console.Error.WriteLine(ex);
            }
        }
    }
}
