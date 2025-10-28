using System.Text.Json.Serialization;
using Country.Application.Dtos;
using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.Users.Commands.UpdateCommand;

public class UpdateCountryCommand : ICommand<bool>
{
    public required string Name { get; set; }

    [JsonIgnore]
    public int Id { get; set; }
}
