
using System.Data;
using CourierJob.Application.Interfaces;
using CourierJob.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using CourierJob.Application.Dtos;
using Mapster;
using Couriers.Infrastructure.Persistence.Context;



public class CourierJobRepository : ICourierJobRepository
{
    private readonly CourierJob.Infrastructure.Persistence.Context.CourierJobDbContext _context;
    private readonly Couriers.Infrastructure.Persistence.Context.CouriersDbContext _context2;
    private readonly ResultCouriers.Infrastructure.Persistence.Context.ResultCouriersDbContext _context3;

    public CourierJobRepository(CourierJobDbContext context, CouriersDbContext context2, ResultCouriers.Infrastructure.Persistence.Context.ResultCouriersDbContext context3)
    {
        _context = context;
        _context2 = context2;
        _context3 = context3;
    }

    public async Task AddAsync(SharedKernel.Domain.Entities.CourierJob City)
    {
        var job = City.Adapt<SharedKernel.Domain.Entities.CourierJob>();
        await _context.CourierJobs.AddAsync(job);
        await _context.SaveChangesAsync();
        int valor = job.Id;
        var allCouriers = await _context2.Couriers
                                 .ToListAsync();

        // .Where(c => c.IsActive)
        var activeCouriers = allCouriers.ToList();

        foreach (var item in activeCouriers)
        {
            var dato = new SharedKernel.Domain.Entities.ResultCouriers();
            dato.IdCourierJob = valor;
            dato.IdCourier = item.Id;
            dato.Currency = "COP";
            dato.Service = "Express";
            dato.Price = 0;
            dato.Attempts = 0;
            dato.Status = (item.IsActive==true) ? 0 : 1;
            await _context3.AddAsync(dato);
            await _context3.SaveChangesAsync();
        }





    }

    public async Task<SharedKernel.Domain.Entities.CourierJob> GetByIdAsync(int id)
    {

        var query = _context.CourierJobs
        .Include(j => j.CourierResults)
            .ThenInclude(r => r.Couriers)
        .Include(j => j.originCity)
        .Include(j => j.originCountry)
        .Include(j => j.destinationCity)
        .Include(j => j.destinationCountry)
        .Where(j => j.Id == id);

        var job = await query.FirstOrDefaultAsync(); // Ejecuta la consulta y obtiene el objeto



        if (job != null)
        {
            if (job.CourierResults != null && job.CourierResults.Any())
            {

                Console.WriteLine($"Resultados encontrados: {job.CourierResults.Count}");


            }
            else
            {


                Console.WriteLine("No se encontraron resultados de mensajería para este trabajo.");
            }
        }

        return job;

    }





    public async Task DeleteAsync(int id)
    {
        var City = await _context.CourierJobs.FindAsync(id);
        if (City != null)
        {
            _context.CourierJobs.Remove(City);
            await _context.SaveChangesAsync();
        }
    }

   


    public IQueryable<SharedKernel.Domain.Entities.CourierJob> GetCarrierJobsAsQueryable()
    {
        var query = _context.CourierJobs

     
        .Include(j => j.CourierResults)
            .ThenInclude(r => r.Couriers)

        .Include(j => j.originCity)
        .Include(j => j.destinationCity)
        .Include(j => j.originCountry)
        .Include(j => j.destinationCountry);

       
        return query;


    }
}
