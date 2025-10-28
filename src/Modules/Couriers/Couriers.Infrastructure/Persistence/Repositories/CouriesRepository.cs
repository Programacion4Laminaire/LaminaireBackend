

using Couriers.Application.Dtos;
using Couriers.Application.Interfaces;
using Couriers.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Couriers.Infrastructure.Persistence.Repositories;
public class CouriesRepository : ICourierRepository
{
    private readonly Couriers.Infrastructure.Persistence.Context.CouriersDbContext _context;

    public CouriesRepository(CouriersDbContext context)
    {
        _context = context;

    }
    public async Task AddAsync(SharedKernel.Domain.Entities.Couriers couriers)
    {
        await _context.AddAsync(couriers);
        await _context.SaveChangesAsync();
    }
    public async Task<SharedKernel.Domain.Entities.Couriers> GetByIdAsync(int id)
    {

        var query = _context.Couriers.Where(j => j.Id == id);

        var job = await query.FirstOrDefaultAsync();
        return job;

    }
    public IQueryable<SharedKernel.Domain.Entities.Couriers> GetCourierAsQueryable()
    {
        var query = _context.Couriers;


       
        return query;


    }
    public async Task UpdateAsync(int id, UpdateCourierDto Couriers)
    {
        var CouriertoUpdate = await _context.Couriers.FindAsync(id);


        if (CouriertoUpdate == null)
        {
            throw new KeyNotFoundException($"El Courier con ID {id} no fue encontrado.");
        }
      
        CouriertoUpdate.Name = Couriers.Name;
        CouriertoUpdate.IsActive = Couriers.IsActive;
        CouriertoUpdate.Url = Couriers.Url;
        CouriertoUpdate.RequiresAuthentication = Couriers.RequiresAuthentication;
        CouriertoUpdate.Username = Couriers.Username;
        CouriertoUpdate.Password = (CouriertoUpdate.Password == Couriers.Password) ? CouriertoUpdate.Password : Couriers.Password;
        CouriertoUpdate.RpaId = Couriers.RpaId;
        
        await _context.SaveChangesAsync();
    }

}
