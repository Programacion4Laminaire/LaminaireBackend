using City.Domain.Entities;
using Country.Domain.Entities;
using SharedKernel.Domain.Entities;
using SharedKernel.Domain.Enums;

namespace CourierJob.Application.Dtos;

public class CourierJobDto
{
    public int Id { get; set; }
    public string? JobName { get; set; }
    public int OriginCityId { get; set; }
    public string? OriginCityName{ get; set; }
    public int DestinationCityId { get; set; }
    public string? DestinationCityName { get; set; }
    public int OriginCountryId { get; set; }
    public string OriginCountryName { get; set; }
    public int DestinationCountryId { get; set; }
    public string DestinationCountryName { get; set; }

    public string? Address { get; set; }
    public string? Zipcode { get; set; }

    public double WeightInKg { get; set; }
    public double HeightInCm { get; set; }
    public double WidthInCm { get; set; }
    public double LengthInCm { get; set; }

    public decimal MerchandiseValueInCop { get; set; }

    public QuotationMode QuotationMode { get; set; }
    public string? QuotationModeName { get; set; }

    public DateTime? CreatedAt { get; set; }

    public ICollection<ResultCouriers.Application.Dtos.ResultCourierDto>? CourierResults { get; set; }
  
}