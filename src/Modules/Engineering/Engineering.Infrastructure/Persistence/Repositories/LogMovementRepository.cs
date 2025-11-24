using Dapper;
using Engineering.Application.Dtos.StateChanges;
using Engineering.Application.Interfaces.Persistence;
using Engineering.Infrastructure.Persistence.Context;
using System.Text;

namespace Engineering.Infrastructure.Persistence.Repositories;

public class LogMovementRepository(EngineeringDbContext context) : ILogMovementRepository
{
    private readonly EngineeringDbContext _context = context;

    public async Task<IEnumerable<StateChangeLogResponseDto>> GetByFilterAsync(StateChangeLogFilterDto filter)
    {
        var sb = new StringBuilder(@"
SELECT 
    IdLog,
    Modulo,
    Entidad,
    IdRegistro,
    IdProyecto,
    IdTarea,
    Consecutivo,
    EstadoAnterior,
    EstadoNuevo,
    Accion,
    Observaciones,
    Usuario,
    Fecha
FROM REQ_LAMINAIRE.dbo.TBL_ING_LOG_MOVIMIENTOS WITH (NOLOCK)
WHERE 1 = 1
");

        var p = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(filter.Modulo))
        {
            sb.AppendLine("  AND Modulo = @Modulo");
            p.Add("@Modulo", filter.Modulo.Trim());
        }

        if (!string.IsNullOrWhiteSpace(filter.Entidad))
        {
            sb.AppendLine("  AND Entidad = @Entidad");
            p.Add("@Entidad", filter.Entidad.Trim());
        }

        if (filter.IdRegistro.HasValue)
        {
            sb.AppendLine("  AND IdRegistro = @IdRegistro");
            p.Add("@IdRegistro", filter.IdRegistro.Value);
        }

        if (filter.IdProyecto.HasValue)
        {
            sb.AppendLine("  AND IdProyecto = @IdProyecto");
            p.Add("@IdProyecto", filter.IdProyecto.Value);
        }

        if (filter.IdTarea.HasValue)
        {
            sb.AppendLine("  AND IdTarea = @IdTarea");
            p.Add("@IdTarea", filter.IdTarea.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.Consecutivo))
        {
            sb.AppendLine("  AND Consecutivo = @Consecutivo");
            p.Add("@Consecutivo", filter.Consecutivo.Trim());
        }

        if (!string.IsNullOrWhiteSpace(filter.EstadoAnterior))
        {
            sb.AppendLine("  AND EstadoAnterior = @EstadoAnterior");
            p.Add("@EstadoAnterior", filter.EstadoAnterior.Trim());
        }

        if (!string.IsNullOrWhiteSpace(filter.EstadoNuevo))
        {
            sb.AppendLine("  AND EstadoNuevo = @EstadoNuevo");
            p.Add("@EstadoNuevo", filter.EstadoNuevo.Trim());
        }

        if (!string.IsNullOrWhiteSpace(filter.Accion))
        {
            sb.AppendLine("  AND Accion = @Accion");
            p.Add("@Accion", filter.Accion.Trim());
        }

        if (!string.IsNullOrWhiteSpace(filter.Usuario))
        {
            sb.AppendLine("  AND Usuario = @Usuario");
            p.Add("@Usuario", filter.Usuario.Trim());
        }

        if (filter.FechaDesde.HasValue)
        {
            sb.AppendLine("  AND Fecha >= @FechaDesde");
            p.Add("@FechaDesde", filter.FechaDesde.Value);
        }

        if (filter.FechaHasta.HasValue)
        {
            var hasta = filter.FechaHasta.Value;
            if (hasta.TimeOfDay == TimeSpan.Zero)
            {
                hasta = hasta.Date.AddDays(1).AddTicks(-1);
            }

            sb.AppendLine("  AND Fecha <= @FechaHasta");
            p.Add("@FechaHasta", hasta);
        }

        // SIN ORDER BY aquí: el orden se maneja en Application con IOrderingQuery
        using var connection = _context.CreateConnection;
        var logs = await connection.QueryAsync<StateChangeLogResponseDto>(sb.ToString(), p);
        return logs;
    }
}
