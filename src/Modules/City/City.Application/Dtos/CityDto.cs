namespace City.Application.Dtos;
public class CityDto
{
    public int Id { get; set; }
    public required string   Name { get; set; }
    public required int CountryId { get; set; }
}