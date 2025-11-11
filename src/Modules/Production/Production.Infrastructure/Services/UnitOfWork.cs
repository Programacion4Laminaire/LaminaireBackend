using Production.Application.Interfaces.Persistence;
using Production.Application.Interfaces.Services;
using Production.Infrastructure.Persistence.Context;
using Production.Infrastructure.Persistence.Repositories;
using System.Data;

namespace Production.Infrastructure.Services;

public class UnitOfWork : IUnitOfWork
{
    private readonly ProductionDbContext _context;
    private IDbConnection _connection;
    private IDbTransaction? _transaction;

    public UnitOfWork(ProductionDbContext context)
    {
        _context = context;
        _connection = _context.CreateConnection;
    }

    private IReprogramLinesRepository? _reprogramLines;
    public IReprogramLinesRepository ReprogramLines =>
        _reprogramLines ??= new ReprogramLinesRepository(_context);

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
