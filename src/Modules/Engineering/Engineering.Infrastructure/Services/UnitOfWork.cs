using Engineering.Application.Interfaces.Persistence;
using Engineering.Application.Interfaces.Services;
using Engineering.Infrastructure.Persistence.Context;
using Engineering.Infrastructure.Persistence.Repositories;
using System.Data;

namespace Engineering.Infrastructure.Services;

public class UnitOfWork : IUnitOfWork
{
    private readonly EngineeringDbContext _context;
    private IProductRepository _product;

    private IDbConnection _connection;
    private IDbTransaction _transaction;

    public UnitOfWork(EngineeringDbContext context)
    {
        _context = context;
        _connection = _context.CreateConnection;
    }

    public IProductRepository Product => _product ??= new ProductRepository(_context);

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
