using SharedKernel.Abstractions.Messaging;

namespace Engineering.Application.UseCases.StateChanges.Queries.ExportQuery;

public record ExportStateChangeLogQuery : IQuery<Stream>
{
    public string Sort { get; init; } = "Fecha";
    public string Order { get; init; } = "desc";
    
    public string? Modulo { get; init; }
    public string? Entidad { get; init; }
    public int? IdRegistro { get; init; }
    public int? IdProyecto { get; init; }
    public int? IdTarea { get; init; }
    public string? Consecutivo { get; init; }
    public string? EstadoAnterior { get; init; }
    public string? EstadoNuevo { get; init; }
    public string? Accion { get; init; }
    public string? Usuario { get; init; }

    public DateTime? FechaDesde { get; init; }
    public DateTime? FechaHasta { get; init; }
}
