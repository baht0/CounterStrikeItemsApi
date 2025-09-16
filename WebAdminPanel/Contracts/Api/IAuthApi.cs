using Refit;
using WebAdminPanel.Models.DTOs;
using WebAdminPanel.Models.DTOs.Auth;

namespace WebAdminPanel.Contracts.Api
{
    public interface IAuthApi
    {
        [Post("/auth/login")]
        Task<ApiStringResponse> Login([Body] AuthDto dto);

        [Post("/auth/logout")]
        Task<string> Logout();
    }
}
