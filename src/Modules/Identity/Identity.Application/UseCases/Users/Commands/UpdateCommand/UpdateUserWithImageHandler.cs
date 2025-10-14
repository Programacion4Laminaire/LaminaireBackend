using Identity.Application.Interfaces.Services;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using BC = BCrypt.Net.BCrypt;

namespace Identity.Application.UseCases.Users.Commands.UpdateCommand;

public class UpdateUserWithImageHandler(
    IUnitOfWork unitOfWork,
    IFileStorageService fileStorage) : ICommandHandler<UpdateUserWithImageCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IFileStorageService _fileStorage = fileStorage;

    public async Task<BaseResponse<bool>> Handle(UpdateUserWithImageCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var user = await _unitOfWork.User.GetByIdAsync(request.UserId);

            if (user == null)
            {
                response.IsSuccess = false;
                response.Message = "Usuario no encontrado";
                return response;
            }

            // Actualizamos campos individuales (igual que el ejemplo anterior)
            user.Identification = request.Identification;
            user.BirthDate = request.BirthDate;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.UserName = request.UserName;
            user.Email = request.Email;
            user.Password = string.IsNullOrWhiteSpace(request.Password)
                ? user.Password
                : BC.HashPassword(request.Password);
            user.State = request.State; // o usar request.State si lo agregas al comando

            // Imagen nueva
            if (request.Image != null && request.Image.Length > 0)
            {
                user.ProfileImagePath = await _fileStorage.SaveUserImageAsync(request.Image, cancellationToken);
            }

            _unitOfWork.User.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            response.IsSuccess = true;
            response.Message = "Usuario actualizado correctamente";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = $"Error al actualizar el usuario: {ex.Message}";
        }

        return response;
    }
}
