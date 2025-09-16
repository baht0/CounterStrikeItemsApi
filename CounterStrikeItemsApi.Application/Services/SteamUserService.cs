using AutoMapper;
using CounterStrikeItemsApi.Application.DTOs;
using CounterStrikeItemsApi.Application.DTOs.SteamUsers;
using CounterStrikeItemsApi.Application.Helpers;
using CounterStrikeItemsApi.Application.Interfaces;
using CounterStrikeItemsApi.Application.Models;
using CounterStrikeItemsApi.Domain.Interfaces;
using CounterStrikeItemsApi.Domain.Models;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http.Json;
using System.Security;
using System.Web;

namespace CounterStrikeItemsApi.Application.Services
{
    public class SteamUserService(
        ISteamUserRepository userRep,
        IRefreshTokenRepository refreshTokenRep,
        IHttpClientFactory httpClientFactory, 
        IConfiguration config) : ISteamUserService
    {
        private readonly ISteamUserRepository _userRep = userRep;
        private readonly IRefreshTokenRepository _refreshTokenRep = refreshTokenRep;
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly IConfiguration _config = config;

        public async Task<PagedResult<SteamUser>> GetUsers(SteamUserFilterQuery query)
        {
            var filter = PredicateBuilder.True<SteamUser>();

            if (!string.IsNullOrWhiteSpace(query.Nickname))
            {
                string decodedStr = HttpUtility.UrlDecode(query.Nickname);
                var lower = decodedStr.ToLower();

                filter = filter.And(i => i.Nickname.ToLower().Contains(lower));
            }

            var (users, total) = await _userRep.GetPaginatedAsync(filter, query.Page, query.PageSize);

            return new PagedResult<SteamUser>
            {
                Rows = users,
                TotalRows = total,
                Page = query.Page,
                PageSize = query.PageSize
            };
        }

        public async Task<SteamUser> GetOrCreateOrUpdateAsync(string steamId, string nickname, string avatarUrl)
        {
            var user = await _userRep.GetBySteamIdAsync(steamId);

            if (user == null)
            {
                user = new SteamUser
                {
                    SteamId = steamId,
                    Nickname = nickname,
                    AvatarUrl = avatarUrl,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                await _userRep.AddAsync(user);
                await _userRep.SaveChangesAsync();
                return user;
            }
            else
            {
                bool updated = false;
                if (user.Nickname != nickname)
                {
                    user.Nickname = nickname;
                    updated = true;
                }
                if (user.AvatarUrl != avatarUrl)
                {
                    user.AvatarUrl = avatarUrl;
                    updated = true;
                }
                if (updated)
                {
                    user.UpdatedAt = DateTime.UtcNow;
                    _userRep.Update(user);
                    await _userRep.SaveChangesAsync();
                    return user;
                }
                return user;
            }
        }

        public async Task UpdateExistUserAsync(SteamUserUpdate dto)
        {
            var existing = await _userRep.GetByIdAsync(dto.Id)
                ?? throw new HttpException(HttpStatusCode.NotFound, "No such user found!");

            existing.IsBanned = dto.IsBanned;
            existing.UpdatedAt = DateTime.UtcNow;

            _userRep.Update(existing);
            await _userRep.SaveChangesAsync();
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var deleted = await _userRep.DeleteAsync(id);
            await _userRep.SaveChangesAsync();
            return deleted;
        }

        // Метод для получения данных профиля из Steam Web API
        public async Task<(string Nickname, string AvatarUrl)> GetSteamProfileAsync(string steamId)
        {
            var key = _config["Steam:ApiKey"];
            var url = $"https://api.steampowered.com/ISteamUser/GetPlayerSummaries/v2/?key={key}&steamids={steamId}";

            var client = _httpClientFactory.CreateClient();
            var resp = await client.GetFromJsonAsync<SteamProfileResponse>(url);

            var player = resp?.Response.Players.FirstOrDefault();

            return player == null ? ("Unknown", "") : (player.Nickname, player.AvatarUrl);
        }

        #region Token
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
            {
                throw new SecurityException("Invalid refresh token");
            }

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
        #endregion
    }
}
