using Identity.Application.Dtos.Users;

namespace Identity.Application.Interfaces.Persistence
{
    public interface ILaminaireUserRepository
    {
        Task<UserCookieDto?> GetUserCookieAsync(string usuarioOrEmail);
    }

}
