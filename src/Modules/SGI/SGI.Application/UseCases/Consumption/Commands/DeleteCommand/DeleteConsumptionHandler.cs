using SGI.Application.Interfaces.Services;
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
            await _uow.Consumption.DeleteAsync(request.ConsumptionId);
            await _uow.SaveChangesAsync();

            res.IsSuccess = true;
            res.Message = "Consumo eliminado";
            res.Data = true;
        }
        catch (Exception ex)
        {
            res.Message = ex.Message;
        }
        return res;
    }
}