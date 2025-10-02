using Mapster;
using SGI.Application.Interfaces.Services;
using SGI.Application.UseCases.Consumption.Commands.CreateCommand;
using SGI.Domain.ValueObjects;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using ConsumptionEntity = SGI.Domain.Entities.Consumption;

namespace SGI.Application.UseCases.Consumption.Commands;

public class CreateConsumptionHandler(IUnitOfWork uow)
  : ICommandHandler<CreateConsumptionCommand, bool>
{
    private readonly IUnitOfWork _uow = uow;

    public async Task<BaseResponse<bool>> Handle(CreateConsumptionCommand req, CancellationToken ct)
    {
        var res = new BaseResponse<bool>();

        try
        {
            // 1) Normalizar el tipo de recurso a español (persistencia estándar)
            var resourceEs = ResourceTypeMaps.ToSpanish(req.ResourceType);

            // 2) Regla de negocio: el valor no puede ser menor que el último anterior (mismo recurso)
            var lastConsumption = await _uow.Consumption.GetLastBeforeAsync(resourceEs, req.ReadingDate);
            if (lastConsumption is not null && req.Value < lastConsumption.Value)
            {
                res.IsSuccess = false;
                res.Message = $"El valor no puede ser menor al del {lastConsumption.ReadingDate:dd/MM/yyyy} ({lastConsumption.Value}).";
                return res;
            }

            // 3) Unicidad por (ResourceType ES, ReadingDate)
            var exists = await _uow.Consumption.ExistsAsync(resourceEs, req.ReadingDate);
            if (exists)
            {
                res.IsSuccess = false;
                res.Message = "Ya existe un consumo para ese recurso y fecha de lectura.";
                return res;
            }

            // 4) Mapear y persistir
            var entity = req.Adapt<ConsumptionEntity>();
            entity.ResourceType = resourceEs; // guardar el recurso en español

            await _uow.Consumption.CreateAsync(entity);
            await _uow.SaveChangesAsync();

            res.IsSuccess = true;
            res.Message = "Consumo creado correctamente";
            res.Data = true;
        }
        catch (Exception ex)
        {
            res.IsSuccess = false;
            res.Message = ex.Message;
        }

        return res;
    }
}
