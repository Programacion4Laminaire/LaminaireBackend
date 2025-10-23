using City.Domain.Entities;
using Country.Domain.Entities;
namespace City.Domain.Entities;

public class CityEntity 
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public int CountryId { get; set; }        
    public required CountryEntity Country { get; set; }  

}