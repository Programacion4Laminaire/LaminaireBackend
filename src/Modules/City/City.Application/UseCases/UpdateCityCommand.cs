using System.Text.Json.Serialization;
using City.Application.Dtos;
using SharedKernel.Abstractions.Messaging;

namespace City.Application.UseCases;

public class UpdateCityCommand : ICommand<bool>
{
    public required string Name { get; set; }
    public int  CountryId { get; set; }

    [JsonIgnore]
    public int Id { get; set; }
}
