using CounterStrikeItemsApi.Application.DTOs.Auth;
using CounterStrikeItemsApi.Application.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security;
using System.Security.Claims;

namespace CounterStrikeItemsApi.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SteamAuthController(
        ISteamUserService userService,
        ITokenService tokenService) : ControllerBase
    {
        private readonly ISteamUserService _userService = userService;
        private readonly ITokenService _tokenService = tokenService;

        [HttpGet("login")]
        public IActionResult SteamLogin()
        {
            var redirectUrl = Url.Action(nameof(SteamCallback), "SteamAuth");
            return Challenge(new AuthenticationProperties { RedirectUri = redirectUrl }, "Steam");
        }

        [HttpGet("callback")]
        public async Task<IActionResult> SteamCallback()
        {
            var result = await HttpContext.AuthenticateAsync("SteamCookie");

            if (!result.Succeeded)
                return Unauthorized();

            var steamIdClaim = result.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (steamIdClaim == null)
                return Unauthorized();

            var steam64Id = steamIdClaim.Split('/').Last();
            var (nickname, avatarUrl) = await _userService.GetSteamProfileAsync(steam64Id);

            var user = await _userService.GetOrCreateOrUpdateAsync(steam64Id, nickname, avatarUrl);
            var (accessToken, refreshToken) = await _tokenService.GenerateTokensAsync(user);

            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7)
            });

            return Ok(new { accessToken });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto dto)
        {
            try
            {
                var (accessToken, refreshToken) = await _tokenService.RefreshAsync(dto.RefreshToken);
                return Ok(new
                {
                    accessToken,
                    refreshToken
                });
            }
            catch (SecurityException)
            {
                return Unauthorized();
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
                return BadRequest("Refresh token not found.");

            try
            {
                await _tokenService.RevokeRefreshTokenAsync(refreshToken);
                Response.Cookies.Delete("refreshToken");
                return Ok(new { message = "Successfully logged out" });
            }
            catch (Exception)
            {
                return BadRequest("Invalid token.");
            }
        }
    }
}
