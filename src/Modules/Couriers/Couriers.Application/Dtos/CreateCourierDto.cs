namespace CarrierJob.Application.Dtos;

public class CreateCourierDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsActive { get; set; }
    public string? Url { get; set; }
    public bool RequiresAuthentication { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? RpaId { get; set; }
}