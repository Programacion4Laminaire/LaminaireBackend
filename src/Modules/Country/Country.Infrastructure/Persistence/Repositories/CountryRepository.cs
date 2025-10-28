
using System.Data;
using Country.Application.Dtos;
using Country.Application.Interfaces;
using Country.Domain.Entities;
using Country.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;



public class CountryRepository : ICountryRepository
{
    private readonly CountryDbContext _context;
    public CountryRepository(CountryDbContext context)
    {
        _context = context;

    }

    public async Task AddAsync(CountryEntity country)
    {
        await _context.Countries.AddAsync(country);
        await _context.SaveChangesAsync();
    }

    public async Task<Country.Domain.Entities.CountryEntity> GetByIdAsync(int id)
    {
        return await _context.Countries.FindAsync(id);
    }

    public async Task<IEnumerable<Country.Domain.Entities.CountryEntity>> GetAllAsync()
    {
        return await _context.Countries.ToListAsync();
    }

   
  

    public async Task DeleteAsync(int id)
    {
        var country = await _context.Countries.FindAsync(id);
        if (country != null)
        {
            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateAsync(int id, UpdateCountryDto country)
    {
        var countryToUpdate = await _context.Countries.FindAsync(id);

      
        if (countryToUpdate == null)
        {
            throw new KeyNotFoundException($"El país con ID {id} no fue encontrado.");
        }

        if (!string.IsNullOrWhiteSpace(country.Name))
        {
           countryToUpdate.Name =country.Name;
        }


        await _context.SaveChangesAsync();
    }

    public IQueryable<CountryEntity> GetAllQueryable()
    {
        return _context.Countries.AsQueryable();
    }
}
