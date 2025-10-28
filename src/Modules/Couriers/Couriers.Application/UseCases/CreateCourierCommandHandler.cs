using Mapster;
using SharedKernel.Abstractions.Encript;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Couriers.Application.UseCases;

public class CreateCourierCommandHandler(Interfaces.ICourierRepository repository,  IDataEncryptor dataEncryptor) : ICommandHandler<CreateCourierCommand, bool>
{
    private readonly Interfaces.ICourierRepository _repository = repository;
    private readonly IDataEncryptor _dataEncryptor=dataEncryptor;

    public async Task<BaseResponse<bool>> Handle(CreateCourierCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var carrierjob = request.Adapt<SharedKernel.Domain.Entities.Couriers>();

            if (request.RequiresAuthentication)
            {
                if(string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.Username))
            {
                    
                    response.IsSuccess = false;
                    response.Message = "El campo Usuario y Contraseña son obligatorios cuando RequiresAuthentication es verdadero.";
                    return response; 
                }
                carrierjob.Password = _dataEncryptor.Encrypt(request.Password);
            }
            else
            {
                carrierjob.Password = null;
                carrierjob.Username = null;
            }
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
