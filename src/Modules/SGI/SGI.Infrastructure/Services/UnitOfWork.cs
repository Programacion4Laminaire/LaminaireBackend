using Microsoft.EntityFrameworkCore.Storage;
using SGI.Application.Interfaces.Persistence;
using SGI.Application.Interfaces.Services;
using SGI.Infrastructure.Persistence.Context;
using SGI.Infrastructure.Persistence.Repositories;
using SharedKernel.Abstractions.Services; // 👈 importante
using System.Data;

namespace SGI.Infrastructure.Services
{
    public class UnitOfWork(ApplicationDbContext context, ICurrentUserService currentUser) : IUnitOfWork
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ICurrentUserService _currentUser = currentUser; // 👈 agregado

        private IConsumptionRepository? _consumption;

        public IConsumptionRepository Consumption =>
            _consumption ??= new ConsumptionRepository(_context, _currentUser); // 👈 corregido

        public IDbTransaction BeginTransaction()
        {
            var transaction = _context.Database.BeginTransaction();
            return transaction.GetDbTransaction();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
