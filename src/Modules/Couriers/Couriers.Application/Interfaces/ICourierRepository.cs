
using Couriers.Application.Dtos;
using SharedKernel.Domain.Entities;
namespace Couriers.Application.Interfaces;
public interface ICourierRepository
{
    // C - Create (Crear)
    Task AddAsync(SharedKernel.Domain.Entities.Couriers couriers);
    Task<SharedKernel.Domain.Entities.Couriers> GetByIdAsync(int id);
    IQueryable<SharedKernel.Domain.Entities.Couriers> GetCourierAsQueryable();
    Task UpdateAsync(int id, UpdateCourierDto city);
}