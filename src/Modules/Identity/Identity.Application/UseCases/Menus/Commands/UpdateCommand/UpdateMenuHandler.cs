using Identity.Application.Interfaces.Services;
using Identity.Domain.Entities;
using Mapster;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Identity.Application.UseCases.Menus.Commands.UpdateCommand
{
    public class UpdateMenuHandler(IUnitOfWork unitOfWork) : ICommandHandler<UpdateMenuCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<BaseResponse<bool>> Handle(UpdateMenuCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var entity = request.Adapt<Menu>();
                entity.Id = request.MenuId;

                _unitOfWork.Menu.UpdateAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                response.IsSuccess = true;
                response.Message = "Actualización exitosa.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
