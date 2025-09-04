namespace Identity.Application.Dtos.Users;

public record UserCookieDto
{
    public string CodUsuario { get; init; } = string.Empty;
    public string Cedula { get; init; } = string.Empty;
    public string NomUsuario { get; init; } = string.Empty;
    public string Proceso { get; init; } = string.Empty;
    public string NomProceso { get; init; } = string.Empty;
    public string SerialDd { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Oc { get; init; } = string.Empty;
    public string Responsable { get; init; } = string.Empty;
    public string ResponsableMto { get; init; } = string.Empty;
    public string FichaT { get; init; } = string.Empty;
}
