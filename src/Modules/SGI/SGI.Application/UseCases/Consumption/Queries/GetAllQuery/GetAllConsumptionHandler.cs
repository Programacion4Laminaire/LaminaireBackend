using Mapster;
using Microsoft.EntityFrameworkCore;
using SGI.Application.Dtos.Consumption;
using SGI.Application.Interfaces.Services;
using SGI.Application.UseCases.Consumption.Queries.GetAllQuery;
using SGI.Domain.ValueObjects;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Abstractions.Services;
using SharedKernel.Commons.Bases;

namespace SGI.Application.UseCases.Consumption.Queries;

public class GetAllConsumptionHandler(IUnitOfWork uow, IOrderingQuery ordering)
  : IQueryHandler<GetAllConsumptionQuery, IEnumerable<ConsumptionResponseDto>>
{
    private readonly IUnitOfWork _uow = uow;
    private readonly IOrderingQuery _ordering = ordering;

    public async Task<BaseResponse<IEnumerable<ConsumptionResponseDto>>> Handle(GetAllConsumptionQuery request, CancellationToken ct)
    {
        var res = new BaseResponse<IEnumerable<ConsumptionResponseDto>>();

        try
        {
            var qry = _uow.Consumption.GetAllQueryable(); // IQueryable<Consumption>

            // Filtro por recurso (el front envía código; convertimos a ES para comparar)
            if (!string.IsNullOrWhiteSpace(request.ResourceType))
            {
                var rtEs = ResourceTypeMaps.ToSpanish(request.ResourceType);
                qry = qry.Where(x => x.ResourceType == rtEs);
            }

            // Rango de fechas de lectura (si ambos vienen)
            if (request.StartReadingDate.HasValue && request.EndReadingDate.HasValue)
            {
                var start = request.StartReadingDate.Value.Date;
                var end = request.EndReadingDate.Value.Date.AddDays(1); // < end
                qry = qry.Where(x => x.ReadingDate >= start && x.ReadingDate < end);
            }

            // Búsqueda por texto (en Note)
            if (!string.IsNullOrEmpty(request.TextFilter))
            {
                qry = qry.Where(x => x.Note != null && x.Note.Contains(request.TextFilter));
            }

            // (Opcional) Rango fechas de auditoría si tu BaseFilters aún lo maneja
            if (!string.IsNullOrEmpty(request.StartDate) && !string.IsNullOrEmpty(request.EndDate))
            {
                var startAudit = Convert.ToDateTime(request.StartDate);
                var endAudit = Convert.ToDateTime(request.EndDate).AddDays(1);
                qry = qry.Where(x => x.AuditCreateDate >= startAudit && x.AuditCreateDate < endAudit);
            }

            // Orden por defecto
            request.Sort ??= "ReadingDate";

            List<SGI.Domain.Entities.Consumption> items;

            if (request.Download)
            {
                // Export: sin paginar
                items = await _ordering.Ordering(request, qry).ToListAsync(ct);
            }
            else
            {
                // Listado: paginado
                items = await _ordering.Ordering(request, qry)
                    .Skip((request.NumPage - 1) * request.Records)
                    .Take(request.Records)
                    .ToListAsync(ct);
            }

            res.IsSuccess = true;
            res.TotalRecords = await qry.CountAsync(ct);
            res.Data = items.Adapt<IEnumerable<ConsumptionResponseDto>>();
            res.Message = "Consulta exitosa.";
        }
        catch (Exception ex)
        {
            res.IsSuccess = false;
            res.Message = ex.Message;
        }

        return res;
    }
}
