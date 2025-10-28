
using CourierJob.Application.Dtos;
using SharedKernel.Domain.Entities;
namespace CourierJob.Application.Interfaces;
public interface ICourierJobRepository
{
    // C - Create (Crear)
    Task AddAsync(SharedKernel.Domain.Entities.CourierJob carrierJob);

    // R - Read (Leer)
    Task<SharedKernel.Domain.Entities.CourierJob> GetByIdAsync(int id);
   
  

    // D - Delete (Eliminar)
    Task DeleteAsync(int id);
    IQueryable<SharedKernel.Domain.Entities.CourierJob> GetCarrierJobsAsQueryable();
}