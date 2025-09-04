using Identity.Application.Dtos.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Interfaces.Services
{
    public interface IUserCookieService
    {
        string BuildCookie(UserCookieDto user);
    }

}
