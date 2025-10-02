using SGI.Application.Interfaces.Services;
using SGI.Domain.ValueObjects;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace SGI.Application.UseCases.Consumption.Commands.UpdateCommand;

public class UpdateConsumptionHandler(IUnitOfWork uow)
  : ICommandHandler<UpdateConsumptionCommand, bool>
{
    private readonly IUnitOfWork _uow = uow;

    public async Task<BaseResponse<bool>> Handle(UpdateConsumptionCommand req, CancellationToken ct)
    {
        var res = new BaseResponse<bool>();

        try
        {
            // 1) Cargar entidad
            var entity = await _uow.Consumption.GetByIdAsync(req.ConsumptionId);
            if (entity is null)
            {
                res.IsSuccess = false;
                res.Message = "Registro no encontrado";
                return res;
            }

            // 2) Normalizar recurso a español y definir fecha efectiva
            var resourceEs = ResourceTypeMaps.ToSpanish(req.ResourceType);
            var readingDateEffective = req.ReadingDate ?? entity.ReadingDate;

            // 3) Regla de negocio: valor no menor que el último anterior (mismo recurso)
            var lastConsumption = await _uow.Consumption.GetLastBeforeAsync(resourceEs, readingDateEffective);
            if (lastConsumption is not null && lastConsumption.Id != entity.Id && req.Value < lastConsumption.Value)
            {
                res.IsSuccess = false;
                res.Message = $"El valor no puede ser menor al del {lastConsumption.ReadingDate:dd/MM/yyyy} ({lastConsumption.Value}).";
                return res;
            }

            // 4) Unicidad por (ResourceType ES, ReadingDate) excluyendo el propio Id
            var exists = await _uow.Consumption.ExistsAsync(resourceEs, readingDateEffective, excludeId: req.ConsumptionId);
            if (exists)
            {
                res.IsSuccess = false;
                res.Message = "Ya existe un consumo para ese recurso y fecha de lectura.";
                return res;
            }

            // 5) Asignación explícita (evitar nulls no deseados)
            entity.ResourceType = resourceEs;      // guardar en español
            entity.Value = req.Value;
            entity.Unit = req.Unit;
            if (req.ReadingDate.HasValue)
                entity.ReadingDate = req.ReadingDate.Value;
            entity.Note = req.Note;

            _uow.Consumption.UpdateAsync(entity);
            await _uow.SaveChangesAsync();

            res.IsSuccess = true;
            res.Message = "Consumo actualizado correctamente";
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
