using Identity.Application.Dtos.Roles;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Identity.Application.UseCases.Roles.Queries.GetAllQuery;

public class GetAllRoleQuery : BaseFilters, IQuery<IEnumerable<RoleResponseDto>> { }
