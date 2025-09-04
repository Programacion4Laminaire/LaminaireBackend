using Identity.Application.Dtos.Users;
using Identity.Application.Interfaces.Services;
using System.Web;

public class UserCookieService : IUserCookieService
{
    public string BuildCookie(UserCookieDto user)
    {
        var keyValues = new Dictionary<string, string?>
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

        return string.Join("&",
            keyValues.Select(kvp =>
                $"{kvp.Key}={Uri.EscapeDataString(kvp.Value ?? string.Empty)}"));
    }

}
