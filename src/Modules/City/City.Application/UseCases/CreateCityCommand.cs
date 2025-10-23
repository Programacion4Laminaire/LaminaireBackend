using System.Windows.Input;
using City.Application.Dtos;
using City.Domain.Entities;
using MediatR;
using SharedKernel.Abstractions.Messaging;
// Este comando lleva los datos necesarios para crear un país.
public class CreateCityCommand : ICommand<bool>
{
    public required string Name { get; set; }
    public required int CountryId { get; set; }


}