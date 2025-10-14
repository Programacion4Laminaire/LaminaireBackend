using Logistics.Application.Interfaces.Persistence;
using System.Data;

namespace Logistics.Application.Interfaces.Services;

public interface IUnitOfWork : IDisposable
{
    IAccessoryEquivalenceRepository AccessoryEquivalence { get; }
    Task SaveChangesAsync();
    IDbTransaction BeginTransaction();
}
