
using System.Data;
using City.Application.Dtos;
using City.Application.Interfaces;
using City.Domain.Entities;
using City.Infrastructure.Persistence.Context;
using Country.Domain.Entities;
using Country.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;



public class CityRepository : ICityRepository
{
    private readonly CityDbContext _context;
    private readonly CountryDbContext _context2;
    public CityRepository(CityDbContext context, CountryDbContext context2)
    {
        _context = context;
        _context2 = context2;

    }

    public async Task AddAsync(CityEntity City)
    {
        await _context.Cities.AddAsync(City);
        await _context.SaveChangesAsync();
    }

    public async Task<City.Domain.Entities.CityEntity> GetByIdAsync(int id)
    {
        return await _context.Cities.FindAsync(id);
    }

    public async Task<IEnumerable<City.Domain.Entities.CityEntity>> GetAllAsync()
    {
        return await _context.Cities.ToListAsync();
    }
  



    public async Task DeleteAsync(int id)
    {
        var City = await _context.Cities.FindAsync(id);
        if (City != null)
        {
            _context.Cities.Remove(City);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateAsync(int id, UpdateCityDto city)
    {
        var CityToUpdate = await _context.Cities.FindAsync(id);
        var CountryToUpdate = await _context2.Countries.FindAsync(city.CountryId);


        if (CityToUpdate == null)
        {
            throw new KeyNotFoundException($"la ciudad con ID {id} no fue encontrado.");
        }

        if (!string.IsNullOrWhiteSpace(city.Name))
        {
           CityToUpdate.Name =city.Name;
        }
        if (city.CountryId !=null)
        {
            CityToUpdate.CountryId = city.CountryId;
        }
        if (CountryToUpdate == null)
        {
            throw new KeyNotFoundException($"El país con ID {city.CountryId} no fue encontrado.");
        }



        await _context.SaveChangesAsync();
    }


    public async Task<IEnumerable<CityEntity>> GetByCountryIdAsync(int countryId)
    {
        return await _context.Cities
            .AsNoTracking()                          
            .Where(c => c.CountryId == countryId)    
            .ToListAsync();                           
    }
    public IQueryable<CityEntity> GetAllQueryable()
    {
        return _context.Cities.AsQueryable();
    }

}
