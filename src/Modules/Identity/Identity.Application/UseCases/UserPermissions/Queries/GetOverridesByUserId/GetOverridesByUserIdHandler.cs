// Application/UseCases/UserPermissions/Queries/GetOverridesByUserId/GetOverridesByUserIdHandler.cs
using Identity.Application.Dtos.UserPermissions;
using Identity.Application.Interfaces.Services;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using SharedKernel.Constants;

namespace Identity.Application.UseCases.UserPermissions.Queries.GetOverridesByUserId;

public sealed class GetOverridesByUserIdHandler(IUnitOfWork uow)
    : IQueryHandler<GetOverridesByUserIdQuery, IEnumerable<UserPermissionOverrideDto>>
{
    private readonly IUnitOfWork _uow = uow;

    public async Task<BaseResponse<IEnumerable<UserPermissionOverrideDto>>> Handle(
        GetOverridesByUserIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<UserPermissionOverrideDto>>();

        try
        {
            var list = await _uow.UserPermission.GetOverridesByUserIdAsync(request.UserId);
            var dto = list.Select(x => new UserPermissionOverrideDto
            {
                PermissionId = x.PermissionId,
                IsGranted = x.IsGranted
            }).ToList();

            response.IsSuccess = true;
            response.Message = GlobalMessages.MESSAGE_QUERY;
            response.Data = dto;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }

        return response;
    }
}
