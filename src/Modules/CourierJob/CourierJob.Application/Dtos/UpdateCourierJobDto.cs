using City.Domain.Entities;
using Country.Domain.Entities;
using SharedKernel.Domain.Enums;

namespace CourierJob.Application.Dtos;

public class UpdateCourierJobDto
{
    public int Id { get; set; }
    public string? JobName { get; set; }
    public int OriginCityId { get; set; }
    public required CityEntity originCity { get; set; }
    public int DestinationCityId { get; set; }
    public required CityEntity destinationCity { get; set; }

    public int OriginCountryId { get; set; }
    public required CountryEntity originCountry { get; set; }
    public int DestinationCuntryId { get; set; }
    public required CountryEntity destinationCountry { get; set; }

    public string? Address { get; set; }
    public string? Zipcode { get; set; }

    public double WeightInKg { get; set; }
    public double HeightInCm { get; set; }
    public double WidthInCm { get; set; }
    public double LengthInCm { get; set; }

    public decimal MerchandiseValueInCop { get; set; }

    public QuotationMode QuotationMode { get; set; }
}