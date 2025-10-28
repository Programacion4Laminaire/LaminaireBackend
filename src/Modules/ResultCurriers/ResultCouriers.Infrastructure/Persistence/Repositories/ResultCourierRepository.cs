

using Microsoft.EntityFrameworkCore;
using ResultCouriers.Application.Dtos;
using ResultCouriers.Application.Interfaces;
using ResultCouriers.Infrastructure.Persistence.Context;
using System.Globalization;

namespace ResultCouriers.Infrastructure.Persistence.Repositories;
public class ResultCourierRepository : IResultCourierRepository
{
    private readonly ResultCouriers.Infrastructure.Persistence.Context.ResultCouriersDbContext _context;

    public ResultCourierRepository(ResultCouriersDbContext context)
    {
        _context = context;

    }
    public async Task AddAsync(SharedKernel.Domain.Entities.ResultCouriers couriers)

    {
        await _context.AddAsync(couriers);
        await _context.SaveChangesAsync();
    }

    public async Task<SharedKernel.Domain.Entities.ResultCouriers?> GetByIdAsync(int id)
    {
        var query = _context.Results.Where(j => j.Id == id);

        var job = await query.FirstOrDefaultAsync();
        return job;
    }

 

    public async Task<SharedKernel.Domain.Entities.ResultCouriers?> GetByIdCourierJobAsync(int id)
    {
        var query = _context.Results.Where(j => j.IdCourierJob == id);

        var job = await query.FirstOrDefaultAsync();
        return job;
    }

    public ResultCouriersDbContext Get_context()
    {
        return _context;
    }

    public async Task<List<SharedKernel.Domain.Entities.ResultCouriers>> GetPendingItemsAsync()
    {

        var pendingItems = await _context.Results.Include(p =>p.CourierJob)
                                                 .ThenInclude(c => c.destinationCity)
                                                 .ThenInclude(city => city.Country)
                                                 .Include(p => p.Couriers)
                                                 .Where( p => p.Status == 0 || (p.Status==4 && p.Attempts<3)).ToListAsync();

       
    
        return pendingItems;
    }

    public IQueryable<SharedKernel.Domain.Entities.ResultCouriers> GetResultCouriersAsQueryable()
    {

        var query = _context.Results;
        return query;
    }

    public  async Task<int> UpdateStatusToProcessingAsync(List<int> itemIds)
    {

        var affectedRows = await _context.Results
         .Where(r => itemIds.Contains(r.Id)) 
         .ExecuteUpdateAsync(setter => setter
             .SetProperty(r => r.Status, 2)
             .SetProperty(r => r.Attempts, 1)
             ); 

       
        return affectedRows;
    }

    public async Task<int> UpdateStatusToProcessingFinalAsync(ResultCourierResponseRPA[] itemIds)
    {
        var result=itemIds.ToList();

        foreach (var item in result)
        {
            var resultCourier = await _context.Results
                                     .FirstOrDefaultAsync(p => p.Id == item.id);
            if (item.status == "fail")
            {

                resultCourier.Status = 4;
                resultCourier.Attempts += 1;
            }
            else
            {
                resultCourier.Status = 3;
                resultCourier.Service = item.service;
                resultCourier.Price = decimal.Parse(item.priceText, CultureInfo.InvariantCulture);
                resultCourier.Currency = item.currency;
                resultCourier.Eta= DateTime.Parse(item.eta);
            }
            _context.Results.Update(resultCourier);
            await _context.SaveChangesAsync();
        }

        throw new NotImplementedException();
    }
}
