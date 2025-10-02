using SGI.Domain.Entities;

namespace SGI.Application.Interfaces.Persistence;

public interface IConsumptionRepository : IGenericRepository<Consumption>
{
    Task<bool> ExistsAsync(string resourceType, DateTime readingDate, int? excludeId = null);
    Task<Consumption?> GetLastBeforeAsync(string resourceType, DateTime readingDate);

}
