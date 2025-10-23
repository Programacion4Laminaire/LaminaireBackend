using Mapster;
using ResultCouriers.Application.Interfaces;
using ResultCouriers.Domain.Entities;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace ResultCouriers.Application.UseCases;

public class CreateResultCourierCommandHandler
    : ICommandHandler<CreateResultCourierCommand, bool>
{
    private readonly ResultCouriers.Application.Interfaces.IResultCourierRepository _repository;

    public CreateResultCourierCommandHandler(IResultCourierRepository repository)
    {
        _repository = repository;
    }

    public async Task<BaseResponse<bool>> Handle(CreateResultCourierCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var entity = request.Adapt<SharedKernel.Domain.Entities.ResultCouriers>();
            await _repository.AddAsync(entity);

            response.IsSuccess = true;
            response.Message = "Registro exitoso";
            response.Data = true;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }

        return response;
    }
}
