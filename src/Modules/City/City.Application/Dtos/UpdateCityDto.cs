namespace City.Application.Dtos;
public class UpdateCityDto
{
    public required string Name { get; set; }
    public int CountryId { get; set; }
}