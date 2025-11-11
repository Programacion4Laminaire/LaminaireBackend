namespace Production.Application.Dtos.ReprogramLines.Responses;

public class ProgrammedLinesResponseDto
{
    public int Id { get; set; }
    public string? BatchNumber { get; set; }//Lote
    public string? OrderNumber { get; set; }
    public string? ProductCode { get; set; }
    public string? ProductDescription { get; set; }
    public decimal Quantity { get; set; }
    public string? Line { get; set; }
    public string? Usp { get; set; }
    public string? Date { get; set; }
    public string? Time { get; set; }
    public string? UserCode { get; set; }
}
