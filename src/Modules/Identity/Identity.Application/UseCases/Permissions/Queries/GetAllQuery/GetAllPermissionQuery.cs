using Identity.Application.Dtos.Permissions;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Identity.Application.UseCases.Permissions.Queries.GetAllQuery;

public class GetAllPermissionQuery : BaseFilters, IQuery<IEnumerable<PermissionCrudResponseDto>> { }
