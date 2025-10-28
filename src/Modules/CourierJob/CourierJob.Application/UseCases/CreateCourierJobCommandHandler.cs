using CourierJob.Application.Interfaces;
using Mapster;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Identity.Application.UseCases.Users.Commands.CreateCommand;

public class CreateCourierJobCommandHandler(ICourierJobRepository repository) : ICommandHandler<CreateCourierJobCommand, bool>
{
    private readonly ICourierJobRepository _repository = repository;

    public async Task<BaseResponse<bool>> Handle(CreateCourierJobCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var carrierjob = request.Adapt<SharedKernel.Domain.Entities.CourierJob>();
            await _repository.AddAsync(carrierjob);

            response.IsSuccess = true;
            response.Message = "Registro exitoso";
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}
