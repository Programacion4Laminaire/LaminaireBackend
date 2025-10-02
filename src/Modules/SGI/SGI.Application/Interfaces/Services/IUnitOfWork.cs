using SGI.Application.Interfaces.Persistence;
using System.Data;

namespace SGI.Application.Interfaces.Services;

public interface IUnitOfWork : IDisposable
{
    IConsumptionRepository Consumption { get; }
   
    Task SaveChangesAsync();
    IDbTransaction BeginTransaction();
}