using Couriers.Application.Dtos;
using Couriers.Application.Interfaces;
using Mapster;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using Couriers.Application.UseCases;
using SharedKernel.Abstractions.Encript;
using SharedKernel.Domain.Entities;


namespace Couriers.Application.UseCases;

public class UpdateUserHandler(ICourierRepository repository, IDataEncryptor dataEncryptor) : ICommandHandler<UpdateCouriersCommand, bool>
{
    private readonly ICourierRepository _repository = repository;
    private readonly IDataEncryptor _dataEncryptor = dataEncryptor;

    public async Task<BaseResponse<bool>> Handle(UpdateCouriersCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var Couriers = request.Adapt<UpdateCourierDto>();

            Couriers.Id = request.Id;
            Couriers.Name = request.Name;
            Couriers.IsActive = request.IsActive;
            Couriers.Url = request.Url;
            Couriers.RequiresAuthentication = request.RequiresAuthentication;
            if (request.RequiresAuthentication)
            {
                if (string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.Username))
                {

                    response.IsSuccess = false;
                    response.Message = "El campo Usuario y Contraseña son obligatorios cuando RequiresAuthentication es verdadero.";
                    return response;
                }
                Couriers.Username = request.Username;
                Couriers.Password = _dataEncryptor.Encrypt(request.Password);
            }
            else
            {
                Couriers.Password = null;
                Couriers.Username = null;
            }
            await _repository.UpdateAsync(request.Id,Couriers);
            response.IsSuccess = true;
            response.Message = "Actualización exitosa";
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}
