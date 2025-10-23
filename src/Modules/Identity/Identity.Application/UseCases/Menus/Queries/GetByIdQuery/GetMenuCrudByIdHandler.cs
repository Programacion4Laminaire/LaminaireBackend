using Identity.Application.Dtos.Menus;
using Identity.Application.Interfaces.Services;
using Mapster;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Identity.Application.UseCases.Menus.Queries.GetByIdQuery
{
    public class GetMenuCrudByIdHandler(IUnitOfWork unitOfWork) : IQueryHandler<GetMenuCrudByIdQuery, MenuCrudByIdResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<BaseResponse<MenuCrudByIdResponseDto>> Handle(GetMenuCrudByIdQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<MenuCrudByIdResponseDto>();

            try
            {
                var entity = await _unitOfWork.Menu.GetByIdAsync(request.MenuId);
                if (entity is null)
                {
                    response.IsSuccess = false;
                    response.Message = "No se encontró el menú.";
                    return response;
                }

                response.IsSuccess = true;
                response.Data = entity.Adapt<MenuCrudByIdResponseDto>();
                response.Message = "Consulta exitosa.";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
