
using System.Windows.Input;
using MediatR;
using SharedKernel.Abstractions.Messaging;
namespace Country.Application.Interfaces;
public class DeleteCountryCommand : ICommand<bool>
{
    public int Id { get; set; }
}
