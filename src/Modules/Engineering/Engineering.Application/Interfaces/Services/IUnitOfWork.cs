using System.Data;

namespace Engineering.Application.Interfaces.Services;

public interface IUnitOfWork : IDisposable
{

    Task SaveChangesAsync();
    IDbTransaction BeginTransaction();
}
