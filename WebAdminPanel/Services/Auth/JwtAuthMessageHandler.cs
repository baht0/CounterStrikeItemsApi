using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using WebAdminPanel.Models;

namespace WebAdminPanel.Services.Auth
{
    public class JwtAuthMessageHandler(
        IJSRuntime js,
        JwtAuthStateProvider authProvider,
        NavigationManager nav) : DelegatingHandler
    {
        private readonly IJSRuntime _js = js;
        private readonly JwtAuthStateProvider _authProvider = authProvider;
        private readonly NavigationManager _nav = nav;

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                // 1. Получаем токен из localStorage
                var token = await _js.InvokeAsync<string>("localStorage.getItem", "authToken");
                // 2. Добавляем токен в заголовок (если есть)
                if (!string.IsNullOrWhiteSpace(token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                // 3. Отправляем запрос
                var response = await base.SendAsync(request, cancellationToken);

                // Обработка 401
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await _js.InvokeVoidAsync("localStorage.removeItem", "authToken");
                    _authProvider.MarkUserAsLoggedOut();
                    _nav.NavigateTo("/login", forceLoad: true);
                }
                // Обработка 400
                if (response.StatusCode == HttpStatusCode.BadRequest)
                    throw new ApiBadRequestException($"BadRequest: {request.RequestUri}");
                // Обработка 404
                if (response.StatusCode == HttpStatusCode.NotFound)
                    throw new ApiNotFoundException($"Resource not found: {request.RequestUri}");

                return response;
            }
            catch (HttpRequestException ex) // Нет подключения к серверу
            {
                Console.WriteLine($"Network error: {ex.Message}");
                throw;
            }
        }
    }
}