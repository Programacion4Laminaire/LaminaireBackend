using SGI.Application.Dtos.Consumption;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace SGI.Application.UseCases.Consumption.Queries.GetAllQuery;

public class GetAllConsumptionQuery : BaseFilters, IQuery<IEnumerable<ConsumptionResponseDto>>
{
    public string? ResourceType { get; set; }
    public bool Download { get; set; } = false;

    // 🔎 Filtros por fecha de lectura
    public DateTime? StartReadingDate { get; set; }
    public DateTime? EndReadingDate { get; set; }

    // 🔎 Texto libre (ej: buscar en Note)
    //public string? TextFilter { get; set; }
}
