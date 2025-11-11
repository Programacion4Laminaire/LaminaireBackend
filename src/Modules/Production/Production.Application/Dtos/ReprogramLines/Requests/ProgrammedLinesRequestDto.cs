namespace Production.Application.Dtos.ReprogramLines.Requests;

public  class ProgrammedLinesRequestDto
{
    public string? OrderNumber { get; set; }
    public string? ProductCode { get; set; }
    public string? BatchNumber { get; set; }
}
