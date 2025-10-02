using Identity.Application.Dtos.Users;
using Identity.Domain.Entities;

namespace Identity.Application.Interfaces.Persistence
{
    public interface ILaminaireUserRepository
    {

        Task InsertAsync(LaminaireUser user);
        Task UpdateAsync(LaminaireUser user);
        Task DeleteByCodigoAsync(string codigo); // 👉 rollback


        // 🔑 Nuevo: actualizar contraseña como Sp_Grabar_Clave
        Task UpdatePasswordAsync(string usuario, string plainPassword, string encryptedPassword);
    }

}
