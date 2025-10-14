using Mapster;
using SGI.Application.Interfaces.Services;
using SGI.Application.UseCases.Consumption.Commands.CreateCommand;
using SGI.Domain.Services; 
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
            var resourceEs = ResourceTypeMaps.ToSpanish(req.ResourceType);
            var sede = req.Sede;
            var readingDate = req.ReadingDate.Date;

            // 🔹 1️⃣ Buscar consumo anterior (misma sede y recurso)
            var lastConsumption = await _uow.Consumption.GetLastBeforeAsync(resourceEs, readingDate, sede);
            decimal? consumoCalculado = null;

            if (lastConsumption is not null)
            {
                // Validar que la lectura no sea menor
                if (req.Value < lastConsumption.Value)
                {
                    res.IsSuccess = false;
                    res.Message = $"El valor no puede ser menor al del {lastConsumption.ReadingDate:dd/MM/yyyy} ({lastConsumption.Value}).";
                    return res;
                }

                // Calcular consumo diario (puede dar 0 si lectura igual)
                consumoCalculado = ConsumptionCalculator.CalculateDaily(resourceEs, req.Value, lastConsumption.Value);
            }

            // 🔹 2️⃣ Validar unicidad por sede, recurso y fecha
            var exists = await _uow.Consumption.ExistsAsync(resourceEs, readingDate, sede: sede);
            if (exists)
            {
                res.IsSuccess = false;
                res.Message = $"Ya existe un consumo para el recurso '{resourceEs}' en la sede '{sede}' con fecha {readingDate:dd/MM/yyyy}.";
                return res;
            }

            // 🔹 3️⃣ Crear y guardar la nueva lectura
            var entity = req.Adapt<ConsumptionEntity>();
            entity.ResourceType = resourceEs;
            entity.Sede = sede;
            entity.DailyConsumption = consumoCalculado;

            await _uow.Consumption.CreateAsync(entity);
            await _uow.SaveChangesAsync();

            // 🔹 4️⃣ Recalcular el siguiente registro (si existe)
            var nextConsumption = await _uow.Consumption.GetNextAfterAsync(resourceEs, readingDate, sede);
            if (nextConsumption is not null)
            {
                // Recalcular su consumo diario con base en esta nueva lectura
                var newDaily = ConsumptionCalculator.CalculateDaily(resourceEs, nextConsumption.Value, req.Value);
                nextConsumption.DailyConsumption = newDaily;

                _uow.Consumption.UpdateAsync(nextConsumption);
                await _uow.SaveChangesAsync();
            }

            // 🔹 5️⃣ Respuesta final
            res.IsSuccess = true;
            res.Message = $"Consumo creado correctamente para el recurso '{resourceEs}' en la sede '{sede}' con fecha {readingDate:dd/MM/yyyy}.";
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
