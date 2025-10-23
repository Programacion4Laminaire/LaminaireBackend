using Identity.Application.Interfaces.Services;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Identity.Application.UseCases.Menus.Commands.DeleteCommand
{
    public class DeleteMenuHandler(IUnitOfWork unitOfWork) : ICommandHandler<DeleteMenuCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<BaseResponse<bool>> Handle(DeleteMenuCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var exists = await _unitOfWork.Menu.GetByIdAsync(request.MenuId);

                if (exists is null)
                {
                    response.IsSuccess = false;
                    response.Message = "El menú no existe en la base de datos.";
                    return response;
                }

                await _unitOfWork.Menu.DeleteAsync(request.MenuId); // soft-delete del genérico
                await _unitOfWork.SaveChangesAsync();

                response.IsSuccess = true;
                response.Message = "Eliminación exitosa.";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
