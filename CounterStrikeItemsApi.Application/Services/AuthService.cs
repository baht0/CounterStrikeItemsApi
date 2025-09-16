using CounterStrikeItemsApi.Application.DTOs.Auth;
using CounterStrikeItemsApi.Application.Helpers;
using CounterStrikeItemsApi.Application.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace CounterStrikeItemsApi.Application.Services
{
    public class AuthService(
        IConfiguration config,
        IPasswordHasher<object> hasher) : IAuthService
    {
        private readonly IConfiguration _config = config;
        private readonly IPasswordHasher<object> _hasher = hasher;

        public string Login(AuthDto dto)
        {
            var username = _config["AdminCredentials:Username"];
            var passwordHash = _config["AdminCredentials:Password"];

            if (dto.Username != username)
                throw new HttpException(HttpStatusCode.Unauthorized, "Incorrect data.");

            var result = _hasher.VerifyHashedPassword(null!, passwordHash!, dto.Password);
            if (result != PasswordVerificationResult.Success)
                throw new HttpException(HttpStatusCode.Unauthorized, "Incorrect data.");

            return JwtHelper.GenerateAccessToken(dto.Username, _config);
        }
    }
}
