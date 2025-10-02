public class LaminaireUser
{
    public string Usuario { get; set; } = null!;
    public string Codigo { get; set; } = null!;
    public string Descripcion { get; set; } = null!;
    public string Clave1 { get; set; } = null!;
    public string E_Mail { get; set; } = null!;
    public string IdUsuario { get; set; } = null!;
    public string Area { get; set; } = "01";
    public string Area_Pem { get; set; } = "PO01";
    public string? Area_Alterna { get; set; } = "";
    public int Habilitado { get; set; } = 1;
    public DateTime FechaClave { get; set; } = DateTime.Now;
}
