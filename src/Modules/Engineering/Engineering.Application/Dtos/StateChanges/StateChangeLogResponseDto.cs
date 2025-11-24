namespace Engineering.Application.Dtos.StateChanges;

public class StateChangeLogResponseDto
{
    public int IdLog { get; set; }
    public string Modulo { get; set; } = default!;
    public string Entidad { get; set; } = default!;
    public int IdRegistro { get; set; }
    public int? IdProyecto { get; set; }
    public int? IdTarea { get; set; }
    public string? Consecutivo { get; set; }
    public string? EstadoAnterior { get; set; }
    public string? EstadoNuevo { get; set; }
    public string Accion { get; set; } = default!;
    public string? Observaciones { get; set; }
    public string Usuario { get; set; } = default!;
    public DateTime Fecha { get; set; }
}
