using Engineering.Application.Interfaces.Services;
using Engineering.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Engineering.Infrastructure.Services;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    private readonly ApplicationDbContext _context = context;


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
