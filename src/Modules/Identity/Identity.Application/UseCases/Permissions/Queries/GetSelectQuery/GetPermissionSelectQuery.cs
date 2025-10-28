using SharedKernel.Abstractions.Messaging;
using SharedKernel.Dtos.Commons;

namespace Identity.Application.UseCases.Permissions.Queries.GetSelectQuery;

public class GetPermissionSelectQuery : IQuery<IEnumerable<SelectResponseDto>> { }
