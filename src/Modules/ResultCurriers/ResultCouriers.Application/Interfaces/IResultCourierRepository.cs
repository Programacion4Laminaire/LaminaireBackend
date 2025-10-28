

using ResultCouriers.Application.Dtos;

namespace ResultCouriers.Application.Interfaces;
public interface IResultCourierRepository
{
    // C - Create (Crear)
    Task AddAsync(SharedKernel.Domain.Entities.ResultCouriers couriers);
    Task<SharedKernel.Domain.Entities.ResultCouriers?> GetByIdAsync(int id);

    Task<SharedKernel.Domain.Entities.ResultCouriers?> GetByIdCourierJobAsync(int id);

    IQueryable<SharedKernel.Domain.Entities.ResultCouriers> GetResultCouriersAsQueryable();
    Task<List<SharedKernel.Domain.Entities.ResultCouriers>> GetPendingItemsAsync();
    Task<int> UpdateStatusToProcessingAsync(List<int> itemIds);
    Task<int> UpdateStatusToProcessingFinalAsync(ResultCourierResponseRPA[] itemIds);

}