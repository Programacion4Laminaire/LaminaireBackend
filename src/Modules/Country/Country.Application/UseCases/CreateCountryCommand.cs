using System.Windows.Input;
using Country.Application.Dtos;
using Country.Domain.Entities;
using MediatR;
using SharedKernel.Abstractions.Messaging;
// Este comando lleva los datos necesarios para crear un país.
public class CreateCountryCommand : ICommand<bool>
{
    public required string Name { get; set; }


}