namespace Country.Application.Interfaces;

using System;
using Country.Application.Dtos;
using Country.Domain.Entities;

public interface ICountryRepository
{
    // C - Create (Crear)
    Task AddAsync(CountryEntity country);

    // R - Read (Leer)
    Task<CountryEntity> GetByIdAsync(int id);
    Task<IEnumerable<CountryEntity>> GetAllAsync();
    // U - Update (Actualizar)
    Task UpdateAsync(int id, UpdateCountryDto Country);

    // D - Delete (Eliminar)
    Task DeleteAsync(int id);
    IQueryable<CountryEntity> GetAllQueryable();

}