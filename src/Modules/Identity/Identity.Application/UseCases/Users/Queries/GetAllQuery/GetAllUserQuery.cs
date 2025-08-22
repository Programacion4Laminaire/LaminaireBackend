using Identity.Application.Dtos.Users;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Identity.Application.UseCases.Users.Queries.GetAllQuery;

public class GetAllUserQuery : BaseFilters, IQuery<IEnumerable<UserResponseDto>> { }
