using Identity.Application.Interfaces.Services;
using Mapster;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using SharedKernel.Constants;
using SharedKernel.Dtos.Commons;

namespace Identity.Application.UseCases.Menus.Queries.GetSelectQuery
{
    public class GetMenuSelectHandler(IUnitOfWork unitOfWork)
        : IQueryHandler<GetMenuSelectQuery, IEnumerable<SelectResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<BaseResponse<IEnumerable<SelectResponseDto>>> Handle(GetMenuSelectQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<IEnumerable<SelectResponseDto>>();

            try
            {
                var menus = await _unitOfWork.Menu.GetAllAsync();

                if (menus is null || !menus.Any())
                {
                    response.IsSuccess = false;
                    response.Message = GlobalMessages.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                response.IsSuccess = true;
                response.Data = menus.Adapt<IEnumerable<SelectResponseDto>>();
                response.Message = GlobalMessages.MESSAGE_QUERY;
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
