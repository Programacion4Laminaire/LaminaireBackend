using Identity.Application.Dtos.Users;
using Identity.Domain.Entities;

namespace Identity.Application.Interfaces.Persistence;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User> UserByEmailAsync(string email);
    Task<UserWithRoleAndPermissionsDto> GetUserWithRoleAndPermissionsAsync(int userId);
    Task<User?> GetByIdentityAndBirthDateAsync(string identification, DateTime birthDate, string userName);
}
