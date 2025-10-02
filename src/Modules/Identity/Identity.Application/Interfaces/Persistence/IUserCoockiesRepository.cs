using Identity.Application.Dtos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Interfaces.Persistence
{
    public interface IUserCoockiesRepository
    {
        Task<UserCookieDto?> GetUserCookieAsync(string usuarioOrEmail);
    }
}
