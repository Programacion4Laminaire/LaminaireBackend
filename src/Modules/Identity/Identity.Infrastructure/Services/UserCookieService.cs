using Identity.Application.Dtos.Users;
using Identity.Application.Interfaces.Services;
using System.Web;


public class UserCookieService : IUserCookieService
{
    public string BuildCookie(UserCookieDto user)
    {
        var pairs = new Dictionary<string, string?>
        {
            ["wCodUsuario"] = user.CodUsuario,
            ["wCedula"] = user.Cedula,
            ["NomUsuario"] = user.NomUsuario,
            ["Proceso"] = user.Proceso,
            ["NomProceso"] = user.NomProceso,
            ["SerialDd"] = user.SerialDd,
            ["Email"] = user.Email,
            ["Oc"] = user.Oc,
            ["Responsable"] = user.Responsable,
            ["ResponsableMto"] = user.ResponsableMto,
            ["FichaT"] = user.FichaT
        };

        // ⚠️ Sin Escape/Encode aquí
        return string.Join("&", pairs.Select(kvp => $"{kvp.Key}={kvp.Value ?? string.Empty}"));
    }
}
