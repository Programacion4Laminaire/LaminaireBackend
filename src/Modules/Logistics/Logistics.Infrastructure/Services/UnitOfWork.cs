using Logistics.Application.Interfaces.Persistence;
using Logistics.Application.Interfaces.Services;
using Logistics.Infrastructure.Persistence.Context;
using Logistics.Infrastructure.Persistence.Repositories;
using System.Data;

namespace Logistics.Infrastructure.Services;

public class UnitOfWork : IUnitOfWork
{
    private readonly LogisticsDbContext _context;
    private IDbConnection _connection;
    private IDbTransaction? _transaction;

    public UnitOfWork(LogisticsDbContext context)
    {
        _context = context;
        _connection = _context.CreateConnection;
    }

    private IAccessoryEquivalenceRepository? _accessoryEquivalence;
    public IAccessoryEquivalenceRepository AccessoryEquivalence =>
        _accessoryEquivalence ??= new AccessoryEquivalenceRepository(_context);

    public IDbTransaction BeginTransaction()
    {
        if (_connection.State != ConnectionState.Open)
            _connection.Open();
        _transaction = _connection.BeginTransaction();
        return _transaction;
    }

    public async Task SaveChangesAsync()
    {
        if (_transaction != null)
        {
            _transaction.Commit();
            await Task.CompletedTask;
        }
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _connection?.Dispose();
        GC.SuppressFinalize(this);
    }
}
