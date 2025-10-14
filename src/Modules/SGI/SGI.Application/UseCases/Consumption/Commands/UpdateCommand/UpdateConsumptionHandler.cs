using SGI.Application.Interfaces.Services;
using SGI.Domain.Services; // 👈 nueva referencia
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
            // 🔹 1️⃣ Cargar entidad existente
            var entity = await _uow.Consumption.GetByIdAsync(req.ConsumptionId);
            if (entity is null)
            {
                res.IsSuccess = false;
                res.Message = $"No se encontró el registro de consumo con ID {req.ConsumptionId}.";
                return res;
            }

            // 🔹 2️⃣ Normalizar datos
            var resourceEs = ResourceTypeMaps.ToSpanish(req.ResourceType);
            var readingDateEffective = req.ReadingDate ?? entity.ReadingDate;
            var sede = string.IsNullOrWhiteSpace(req.Sede) ? entity.Sede : req.Sede;

            // 🔹 3️⃣ Validar que la lectura no sea menor que la anterior
            var lastConsumption = await _uow.Consumption.GetLastBeforeAsync(resourceEs, readingDateEffective, sede);
            if (lastConsumption is not null &&
                lastConsumption.Id != entity.Id &&
                req.Value < lastConsumption.Value)
            {
                res.IsSuccess = false;
                res.Message =
                    $"El valor para el recurso '{resourceEs}' en la sede '{sede}' no puede ser menor al del " +
                    $"{lastConsumption.ReadingDate:dd/MM/yyyy} ({lastConsumption.Value:N3}).";
                return res;
            }

            // 🔹 4️⃣ Validar unicidad
            var exists = await _uow.Consumption.ExistsAsync(resourceEs, readingDateEffective, excludeId: req.ConsumptionId, sede: sede);
            if (exists)
            {
                res.IsSuccess = false;
                res.Message =
                    $"Ya existe un consumo para el recurso '{resourceEs}' en la sede '{sede}' con fecha {readingDateEffective:dd/MM/yyyy}.";
                return res;
            }

            // 🔹 5️⃣ Recalcular consumo diario (si hay un anterior)
            decimal? consumoCalculado = null;
            if (lastConsumption is not null && req.Value >= lastConsumption.Value)
            {
                consumoCalculado = ConsumptionCalculator.CalculateDaily(resourceEs, req.Value, lastConsumption.Value);
            }

            // 🔹 6️⃣ Actualizar entidad
            entity.ResourceType = resourceEs;
            entity.Value = req.Value;
            entity.Unit = req.Unit;
            entity.Sede = sede;
            entity.Note = req.Note;
            entity.ReadingDate = readingDateEffective;
            entity.DailyConsumption = consumoCalculado;

            _uow.Consumption.UpdateAsync(entity);
            await _uow.SaveChangesAsync();

            // 🔹 7️⃣ Recalcular el siguiente (si existe)
            var next = await _uow.Consumption.GetNextAfterAsync(resourceEs, readingDateEffective, sede);
            if (next is not null)
            {
                next.DailyConsumption = ConsumptionCalculator.CalculateDaily(resourceEs, next.Value, req.Value);
                _uow.Consumption.UpdateAsync(next);
                await _uow.SaveChangesAsync();
            }

            // 🔹 8️⃣ Éxito
            res.IsSuccess = true;
            res.Message =
                $"Consumo actualizado correctamente para el recurso '{resourceEs}' en la sede '{sede}' con fecha {readingDateEffective:dd/MM/yyyy}.";
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
