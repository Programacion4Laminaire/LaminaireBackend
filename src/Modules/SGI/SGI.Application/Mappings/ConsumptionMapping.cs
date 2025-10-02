
using Mapster;
using SGI.Application.Dtos.Consumption;
using SGI.Application.UseCases.Consumption.Commands.CreateCommand;
using SGI.Application.UseCases.Consumption.Commands.UpdateCommand;
using SGI.Domain.Entities;

namespace SGI.Application.Mappings;

public class ConsumptionMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Consumption, ConsumptionResponseDto>()
            .Map(d => d.ConsumptionId, s => s.Id)            
            .TwoWays();

        config.NewConfig<Consumption, ConsumptionByIdResponseDto>()
            .Map(d => d.ConsumptionId, s => s.Id)
            .TwoWays();

        config.NewConfig<CreateConsumptionCommand, Consumption>();
        config.NewConfig<UpdateConsumptionCommand, Consumption>();
    }
}
