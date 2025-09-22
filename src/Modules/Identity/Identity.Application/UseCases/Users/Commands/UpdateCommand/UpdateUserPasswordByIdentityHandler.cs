using Identity.Application.Interfaces.Services;
using Mapster;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using BC = BCrypt.Net.BCrypt;

namespace Identity.Application.UseCases.Users.Commands.UpdateCommand;

public class UpdateUserPasswordByIdentityHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateUserPasswordByIdentityCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<BaseResponse<bool>> Handle(UpdateUserPasswordByIdentityCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            // 1) Buscar usuario por identificación, fecha de nacimiento y username
            var user = await _unitOfWork.User.GetByIdentityAndBirthDateAsync(
                request.Identification,
                request.BirthDate,
                request.UserName
            );

            if (user is null)
            {
                response.Message = "Usuario, identificación o fecha de nacimiento incorrecta.";
                return response;
            }

            // 2) Mapear los datos del request al user existente
            request.Adapt(user);

            // 3) Hashear nueva contraseña
            user.Password = BC.HashPassword(request.NewPassword);

            // 4) Persistir cambios
            _unitOfWork.User.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            response.IsSuccess = true;
            response.Message = "Contraseña actualizada correctamente.";
        }
        catch (Exception ex)
        {
            response.Message = $"Error al actualizar contraseña: {ex.Message}";
        }

        return response;
    }
}