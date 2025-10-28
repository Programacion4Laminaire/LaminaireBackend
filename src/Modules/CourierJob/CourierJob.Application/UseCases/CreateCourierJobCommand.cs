
using City.Domain.Entities;
using Country.Domain.Entities;
using SharedKernel.Domain.Enums;
using MediatR;
using SharedKernel.Abstractions.Messaging;
public class CreateCourierJobCommand : ICommand<bool>
{
    public string? JobName { get; set; }
    public int OriginCityId { get; set; }
    public int DestinationCityId { get; set; }

    public int OriginCountryId { get; set; }
    public int DestinationCountryId { get; set; }

    public string? Address { get; set; }
    public string? Zipcode { get; set; }

    public double WeightInKg { get; set; }
    public double HeightInCm { get; set; }
    public double WidthInCm { get; set; }
    public double LengthInCm { get; set; }

    public decimal MerchandiseValueInCop { get; set; }

    public QuotationMode QuotationMode { get; set; }


}