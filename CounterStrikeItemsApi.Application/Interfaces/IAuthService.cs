using CounterStrikeItemsApi.Application.DTOs.Auth;

namespace CounterStrikeItemsApi.Application.Interfaces
{
    public interface IAuthService
    {
        string Login(AuthDto dto);
    }
}
