using SharedKernel.Abstractions.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGI.Application.UseCases.Consumption.Commands.DeleteCommand;

public record DeleteConsumptionCommand(int ConsumptionId) : ICommand<bool>;
