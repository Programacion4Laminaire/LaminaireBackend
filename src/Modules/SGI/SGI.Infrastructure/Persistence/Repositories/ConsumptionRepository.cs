
using Microsoft.EntityFrameworkCore;
using SGI.Application.Interfaces.Persistence;
using SGI.Domain.Entities;
using SGI.Infrastructure.Persistence.Context;

namespace SGI.Infrastructure.Persistence.Repositories;

public class ConsumptionRepository(ApplicationDbContext ctx)
  : GenericRepository<Consumption>(ctx), IConsumptionRepository
{
    private readonly ApplicationDbContext _ctx = ctx;

    public async Task<bool> ExistsAsync(string resourceType, DateTime readingDate, int? excludeId = null)
    {
        var dateOnly = readingDate.Date;

        var q = _ctx.Set<Consumption>()
            .Where(x => x.ResourceType == resourceType
                     && x.ReadingDate == dateOnly
                     && x.AuditDeleteDate == null
                     && x.AuditDeleteUser == null);

        if (excludeId.HasValue) q = q.Where(x => x.Id != excludeId.Value);
        return await q.AnyAsync();
    }
    public async Task<Consumption?> GetLastBeforeAsync(string resourceType, DateTime readingDate)
    {
        var dateOnly = readingDate.Date;

        return await _ctx.Set<Consumption>()
            .Where(x => x.ResourceType == resourceType
                     && x.ReadingDate < dateOnly
                     && x.AuditDeleteDate == null
                     && x.AuditDeleteUser == null)
            .OrderByDescending(x => x.ReadingDate)
            .FirstOrDefaultAsync();
    }

}
