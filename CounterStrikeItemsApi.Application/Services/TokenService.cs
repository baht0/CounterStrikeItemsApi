using CounterStrikeItemsApi.Application.Helpers;
using CounterStrikeItemsApi.Application.Interfaces;
using CounterStrikeItemsApi.Domain.Interfaces;
using CounterStrikeItemsApi.Domain.Models;
using Microsoft.Extensions.Configuration;
using System.Security;

namespace CounterStrikeItemsApi.Application.Services
{
    public class TokenService(
        IRefreshTokenRepository refreshTokenRep,
        IConfiguration config) : ITokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRep = refreshTokenRep;
        private readonly IConfiguration _config = config;

        public async Task<(string accessToken, string refreshToken)> GenerateTokensAsync(SteamUser user)
        {
            var accessToken = JwtHelper.GenerateAccessToken(user, _config);
            var refreshToken = JwtHelper.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            await _refreshTokenRep.AddAsync(refreshTokenEntity);
            await _refreshTokenRep.SaveChangesAsync();

            return (accessToken, refreshToken);
        }

        public async Task<(string accessToken, string refreshToken)> RefreshAsync(string refreshToken)
        {
            var existingToken = await _refreshTokenRep.GetByTokenAsync(refreshToken);

            if (existingToken == null || existingToken.IsRevoked || existingToken.ExpiresAt < DateTime.UtcNow)
                throw new SecurityException("Invalid refresh token");

            var user = existingToken.User;

            // Отозвать старый токен
            existingToken.IsRevoked = true;
            _refreshTokenRep.Update(existingToken);
            await _refreshTokenRep.SaveChangesAsync();

            // Создать новые токены
            var accessToken = JwtHelper.GenerateAccessToken(user, _config);
            var newRefreshToken = JwtHelper.GenerateRefreshToken();

            var newRefreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                Token = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                IsRevoked = false
            };

            await _refreshTokenRep.AddAsync(newRefreshTokenEntity);
            await _refreshTokenRep.SaveChangesAsync();

            return (accessToken, newRefreshToken);
        }

        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            var tokenEntity = await _refreshTokenRep.GetByTokenAsync(refreshToken);
            if (tokenEntity == null || tokenEntity.IsRevoked)
                throw new SecurityException("Invalid token");

            tokenEntity.IsRevoked = true;
            await _refreshTokenRep.SaveChangesAsync();
        }
    }
}
