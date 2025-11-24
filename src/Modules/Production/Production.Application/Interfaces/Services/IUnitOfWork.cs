using Production.Application.Interfaces.Persistence;
using System.Data;

namespace Production.Application.Interfaces.Services;

public interface IUnitOfWork : IDisposable
{
    IReprogramLinesRepository ReprogramLines { get; }
    Task SaveChangesAsync();
    IDbTransaction BeginTransaction();
}
