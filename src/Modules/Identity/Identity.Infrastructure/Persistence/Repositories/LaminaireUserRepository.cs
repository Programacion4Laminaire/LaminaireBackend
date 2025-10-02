using Dapper;
using Identity.Application.Interfaces.Persistence;
using Identity.Domain.Entities;
using Identity.Infrastructure.Persistence.Context;
using Microsoft.Data.SqlClient;

namespace Identity.Infrastructure.Persistence.Repositories;

public class LaminaireUserRepository(LaminaireDbContext dbContext) : ILaminaireUserRepository
{
    private readonly LaminaireDbContext _dbContext = dbContext;

    public async Task InsertAsync(LaminaireUser user)
    {
        var sql = @"
    INSERT INTO [Control_Solicitud].[dbo].[TBL_USUARIOS]
        (Usuario, Codigo, Descripcion, Clave, Clave1, E_Mail, IdUsuario,
         Area, Area_Pem, Area_Alterna, Responsable, Asigna, Audita, Cierra, EnLinea,
         Responsablemto, ResponsableCalidad, Ficha_T, CierraMc, ResponsableMc,
         Habilitado, AnalisisCausas, Ingeniero, Asesor, EficaciaAcciones, Oc,
         E_MailAlterno, Seleccion, Serial_DD, Ultima_Ejecucion, Version_Office, FechaClave)
    VALUES
        (@Usuario, @Codigo, @Descripcion, '', @Clave1, @E_Mail, @IdUsuario,
         '01', 'PO01', '', 
         0, 0, 0, 0, 0,
         0, 0, 0, 0, 0,
         @Habilitado, 0, 0, 0, 0, 0,
         @E_Mail, 0, '', NULL, '2010', GETDATE())";

        using var connection = _dbContext.CreateConnection;
        await connection.ExecuteAsync(sql, user);
    }


    public async Task UpdateAsync(LaminaireUser user)
    {
        var sql = @"
            UPDATE [Control_Solicitud].[dbo].[TBL_USUARIOS]
            SET Usuario = @Usuario,
                Descripcion = @Descripcion,
                Clave1 = @Clave1,
                E_Mail = @E_Mail,
                Habilitado = @Habilitado,
                FechaClave = GETDATE()
            WHERE Codigo = @Codigo";

        using var connection = _dbContext.CreateConnection;
        await connection.ExecuteAsync(sql, user);
    }

    public async Task UpdatePasswordAsync(string usuario, string plainPassword, string encryptedPassword)
    {
        using var connection = (SqlConnection)_dbContext.CreateConnection; // 👈 casteo
        await connection.OpenAsync(); // ✅ ahora sí funciona

        using var tran = connection.BeginTransaction();

        try
        {
            // 1) Update en TBL_USUARIOS
            await connection.ExecuteAsync(@"
                UPDATE TBL_USUARIOS 
                SET Clave1 = @Encrypted, FechaClave = GETDATE()
                WHERE Usuario = @Usuario",
                new { Usuario = usuario, Encrypted = encryptedPassword }, tran);

            // 2) Update en OFIMA si existe
            await connection.ExecuteAsync(@"
                UPDATE CONTROL_OFIMAEnterprise..MTUSUARIO
                SET PASSWORD = @Plain
                WHERE CODUSUARIO = REPLACE(REPLACE(RTRIM(LTRIM(@Usuario)),'HEORTIZ','EORTIZ'),'GHENRIQUEZ','GENRIQUEZ')",
                new { Usuario = usuario, Plain = plainPassword }, tran);

            // 3) Insertar en histórico
            await connection.ExecuteAsync(@"
                INSERT INTO TBL_HISTORICO_CLAVES (Usuario, Clave, Numero)
                VALUES (@Usuario, @Encrypted, 1)",
                new { Usuario = usuario, Encrypted = encryptedPassword }, tran);

            tran.Commit();
        }
        catch
        {
            tran.Rollback();
            throw;
        }
    }
    public async Task DeleteByCodigoAsync(string codigo)
    {
        var sql = @"
            DELETE FROM [Control_Solicitud].[dbo].[TBL_USUARIOS]
            WHERE Codigo = @Codigo";

        using var connection = _dbContext.CreateConnection;
        await connection.ExecuteAsync(sql, new { Codigo = codigo });
    }
}
