using SGI.Domain.Entities;

namespace SGI.Application.Interfaces.Persistence;

public interface IConsumptionRepository : IGenericRepository<Consumption>
{
    Task<bool> ExistsAsync(string resourceType, DateTime readingDate, int? excludeId = null, string? sede = null);
    Task<Consumption?> GetLastBeforeAsync(string resourceType, DateTime readingDate, string? sede = null);
    Task<Consumption?> GetNextAfterAsync(string resourceType, DateTime readingDate, string? sede = null);


}
