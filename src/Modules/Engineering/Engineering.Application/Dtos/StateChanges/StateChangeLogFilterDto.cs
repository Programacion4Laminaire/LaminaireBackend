namespace Engineering.Application.Dtos.StateChanges;

public class StateChangeLogFilterDto
{
    public string? Modulo { get; set; }
    public string? Entidad { get; set; }
    public int? IdRegistro { get; set; }
    public int? IdProyecto { get; set; }
    public int? IdTarea { get; set; }
    public string? Consecutivo { get; set; }
    public string? EstadoAnterior { get; set; }
    public string? EstadoNuevo { get; set; }
    public string? Accion { get; set; }
    public string? Usuario { get; set; }
    public DateTime? FechaDesde { get; set; }
    public DateTime? FechaHasta { get; set; }
}
