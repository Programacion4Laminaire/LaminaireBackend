using SGI.Application.Interfaces.Services;
using SGI.Domain.Services; // 👈 Nueva referencia
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace SGI.Application.UseCases.Consumption.Commands.DeleteCommand;

public class DeleteConsumptionHandler(IUnitOfWork uow)
  : ICommandHandler<DeleteConsumptionCommand, bool>
{
    private readonly IUnitOfWork _uow = uow;

    public async Task<BaseResponse<bool>> Handle(DeleteConsumptionCommand request, CancellationToken ct)
    {
        var res = new BaseResponse<bool>();

        try
        {
            // 🔹 1️⃣ Obtener el registro a eliminar
            var entity = await _uow.Consumption.GetByIdAsync(request.ConsumptionId);
            if (entity is null)
            {
                res.IsSuccess = false;
                res.Message = "No se encontró el registro de consumo.";
                return res;
            }

            var resource = entity.ResourceType;
            var sede = entity.Sede;
            var readingDate = entity.ReadingDate;

            // 🔹 2️⃣ Buscar el anterior y el siguiente registro (válidos)
            var prev = await _uow.Consumption.GetLastBeforeAsync(resource, readingDate, sede);
            var next = await _uow.Consumption.GetNextAfterAsync(resource, readingDate, sede);

            // 🔹 3️⃣ Eliminar lógicamente el actual
            await _uow.Consumption.DeleteAsync(request.ConsumptionId);
            await _uow.SaveChangesAsync();

            // 🔹 4️⃣ Si existe un siguiente, recalcular su consumo diario
            if (next is not null)
            {
                if (prev is not null)
                {
                    // nuevo consumo diario: (lectura siguiente - lectura anterior)
                    next.DailyConsumption = ConsumptionCalculator.CalculateDaily(resource, next.Value, prev.Value);
                }
                else
                {
                    // si no hay anterior, el siguiente ya no tiene referencia
                    next.DailyConsumption = null;
                }

                _uow.Consumption.UpdateAsync(next);
                await _uow.SaveChangesAsync();
            }

            // 🔹 5️⃣ Respuesta OK
            res.IsSuccess = true;
            res.Message = $"Consumo del {readingDate:dd/MM/yyyy} eliminado correctamente.";
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
