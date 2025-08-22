using Identity.Application.Dtos.Users;
using SharedKernel.Abstractions.Messaging;

namespace Identity.Application.UseCases.Users.Queries.GetByIdQuery;

public class GetUserByIdQuery : IQuery<UserByIdResponseDto>
{
    public int UserId { get; set; }
}
