using City.Domain.Entities;
using Country.Domain.Entities;
namespace SharedKernel.Domain.Entities;
public class CourierJob
{
     public int Id { get; set; }
    public string? JobName { get; set; }
    public int OriginCityId { get; set; }
    public  CityEntity? originCity { get; set; }
    public int DestinationCityId { get; set; }
    public CityEntity? destinationCity { get; set; }

    public int OriginCountryId { get; set; }
    public  CountryEntity? originCountry { get; set; }
    public int DestinationCountryId { get; set; }
    public CountryEntity? destinationCountry { get; set; }
    
    public string? Address { get; set; }
    public string? Zipcode { get; set; }

    public double WeightInKg { get; set; } 
    public double HeightInCm { get; set; } 
    public double WidthInCm { get; set; } 
    public double LengthInCm { get; set; } 

    public decimal MerchandiseValueInCop { get; set; }

    public SharedKernel.Domain.Enums.QuotationMode QuotationMode { get; set; }

    public ICollection<ResultCouriers>? CourierResults { get; set; }

    public DateTime CreatedAt { get; set; }

    public int Status { get; set; }
    public CourierJob()
    {
        CreatedAt = DateTime.UtcNow;
        Status = 0;
    }


}