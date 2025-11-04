using Identity.Application.Interfaces.Services;
using Mapster;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using SharedKernel.Constants;
using SharedKernel.Dtos.Commons;

namespace Identity.Application.UseCases.Permissions.Queries.GetSelectQuery;

public class GetPermissionSelectHandler(IUnitOfWork uow)
    : IQueryHandler<GetPermissionSelectQuery, IEnumerable<SelectResponseDto>>
{
    public async Task<BaseResponse<IEnumerable<SelectResponseDto>>> Handle(GetPermissionSelectQuery request, CancellationToken ct)
    {
        var response = new BaseResponse<IEnumerable<SelectResponseDto>>();

        try
        {
            var list = await uow.Permission.GetAllAsync();
            if (list is null)
            {
                response.IsSuccess = false;
                response.Message = GlobalMessages.MESSAGE_QUERY_EMPTY;
                return response;
            }

            response.IsSuccess = true;
            response.Data = list.Adapt<IEnumerable<SelectResponseDto>>();
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
