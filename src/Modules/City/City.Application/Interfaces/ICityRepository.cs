
using City.Application.Dtos;
using City.Domain.Entities;
using Country.Domain.Entities;
namespace City.Application.Interfaces;
public interface ICityRepository
{
    // C - Create (Crear)
    Task AddAsync(CityEntity Country);

    // R - Read (Leer)
    Task<CityEntity> GetByIdAsync(int id);
    Task<IEnumerable<CityEntity>> GetAllAsync();
    // U - Update (Actualizar)
    Task UpdateAsync(int id, UpdateCityDto city);
   

    // D - Delete (Eliminar)
    Task DeleteAsync(int id);
    Task<IEnumerable<CityEntity>> GetByCountryIdAsync(int countryId);
    IQueryable<CityEntity> GetAllQueryable();
}