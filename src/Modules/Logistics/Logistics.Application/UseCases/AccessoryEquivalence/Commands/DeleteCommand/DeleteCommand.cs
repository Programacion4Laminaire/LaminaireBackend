using SharedKernel.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logistics.Application.UseCases.AccessoryEquivalence.Commands.DeleteCommand
{
    public class DeleteCommand : ICommand<bool>
    {
        public int Id { get; init; }
    }
}
