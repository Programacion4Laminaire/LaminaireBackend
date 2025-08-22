using Identity.Application.Interfaces.Services;
using Identity.Domain.Entities;
using Mapster;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using BC = BCrypt.Net.BCrypt;

namespace Identity.Application.UseCases.Users.Commands.CreateCommand;

public class CreateUserWithImageHandler(
    IUnitOfWork unitOfWork,
    IFileStorageService fileStorage) : ICommandHandler<CreateUserWithImageCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IFileStorageService _fileStorage = fileStorage;

    public async Task<BaseResponse<bool>> Handle(CreateUserWithImageCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var user = request.Adapt<User>();
            user.Password = BC.HashPassword(request.Password);
            user.State = "1";

            if (request.Image != null && request.Image.Length > 0)
            {
                user.ProfileImagePath = await _fileStorage.SaveUserImageAsync(request.Image, cancellationToken);
            }
            else
            {
                user.ProfileImagePath = "/uploads/users/default-user.png";
            }

            await _unitOfWork.User.CreateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            response.IsSuccess = true;
            response.Message = "Usuario creado correctamente";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = $"Error al crear el usuario: {ex.Message}";
        }

        return response;
    }
}
