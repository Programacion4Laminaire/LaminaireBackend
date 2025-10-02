using Microsoft.EntityFrameworkCore.Storage;
using SGI.Application.Interfaces.Persistence;
using SGI.Application.Interfaces.Services;
using SGI.Infrastructure.Persistence.Context;
using SGI.Infrastructure.Persistence.Repositories;
using System.Data;

namespace SGI.Infrastructure.Services;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    private readonly ApplicationDbContext _context = context;
    private readonly IConsumptionRepository _consumption = null!;


  

    public IConsumptionRepository Consumption => _consumption ?? new ConsumptionRepository(_context);

    public IDbTransaction BeginTransaction()
    {
        var transaction = _context.Database.BeginTransaction();
        return transaction.GetDbTransaction();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}