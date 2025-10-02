using Dapper;
using Identity.Application.Dtos.Users;
using Identity.Application.Interfaces.Persistence;
using Identity.Domain.Entities;
using Identity.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

public class UserCoockiesRepository : IUserCoockiesRepository
{
    private readonly LaminaireDbContext _context;

    public UserCoockiesRepository(LaminaireDbContext context)
    {
        _context = context;
    }

    public async Task<UserCookieDto?> GetUserCookieAsync(string usuarioOrEmail)
    {
        var sql = @"
        SELECT TOP 1 
        LTRIM(RTRIM(Usu.Usuario))        AS CodUsuario,
        LTRIM(RTRIM(Usu.Codigo))         AS Cedula,
        LTRIM(RTRIM(Usu.Descripcion))    AS NomUsuario,
        LTRIM(RTRIM(Usu.Area))           AS Proceso,
        LTRIM(RTRIM(Are.Descripcion))    AS NomProceso,
        LTRIM(RTRIM(Usu.Serial_DD))      AS SerialDd,
        LTRIM(RTRIM(Usu.E_Mail))         AS Email,
        IIF(LTRIM(RTRIM(CAST(Usu.Oc AS varchar)))='0','False','True') AS Oc,
        LTRIM(RTRIM(CAST(Usu.Responsable AS varchar))) AS Responsable,
        LTRIM(RTRIM(CAST(Usu.ResponsableMto AS varchar))) AS ResponsableMto,
        LTRIM(RTRIM(CAST(Usu.Ficha_T AS varchar))) AS FichaT
    FROM Tbl_Usuarios Usu
    JOIN Req_Laminaire.dbo.Tbl_Areas Are 
        ON Usu.Area = Are.Codigo
    WHERE Usu.Habilitado = 1
      AND (Usu.Usuario = @usuarioOrEmail OR Usu.E_Mail = @usuarioOrEmail)";

        using var connection = _context.CreateConnection;
        return await connection.QueryFirstOrDefaultAsync<UserCookieDto>(sql, new { usuarioOrEmail });
    }


}
