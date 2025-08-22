using Identity.Application.Dtos.UserRole;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Identity.Application.UseCases.UserRoles.Queries.GetAllQuery;

public class GetAllUserRoleQuery : BaseFilters, IQuery<IEnumerable<UserRoleResponseDto>> { }
