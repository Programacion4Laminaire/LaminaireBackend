using Identity.Application.Interfaces.Services;
using Identity.Domain.Entities;
using Mapster;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Identity.Application.UseCases.Menus.Commands.CreateCommand
{
    public class CreateMenuHandler(IUnitOfWork unitOfWork) : ICommandHandler<CreateMenuCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<BaseResponse<bool>> Handle(CreateMenuCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<bool>();

            try
            {
                var entity = request.Adapt<Menu>();
                await _unitOfWork.Menu.CreateAsync(entity);
                await _unitOfWork.SaveChangesAsync();

                response.IsSuccess = true;
                response.Message = "Registro exitoso.";
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
