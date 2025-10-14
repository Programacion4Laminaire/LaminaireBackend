using Logistics.Application.Dtos.AccessoryEquivalence;
using SharedKernel.Dtos.Commons;

namespace Logistics.Application.Interfaces.Persistence;

public interface IAccessoryEquivalenceRepository
{
    Task<(IEnumerable<AccessoryEquivalenceResponseDto> Items, int TotalRecords)>
        GetPagedAsync(int records, int numPage, string sort, string order, int numFilter, string? textFilter);

    Task<AccessoryEquivalenceResponseDto?> GetByIdAsync(int id);
    Task<bool> CreateAsync(AccessoryEquivalenceCreateRequestDto dto);
    Task<bool> UpdateAsync(AccessoryEquivalenceUpdateRequestDto dto);
    Task<bool> DeleteAsync(int id);

    /// <summary>Obtiene la descripción desde MTMERCIA por código.</summary>
    Task<string?> GetDescripcionByCodigoAsync(string codigo);
    Task<IEnumerable<SelectResponseDto>> GetMerciaSelectAsync(string? searchTerm, string? kind);
    Task<bool> ExistsAsync(string codigoPT, string codigoMP, int? excludeId = null);
}
