using Engineering.Application.Dtos.StateChanges;
using Engineering.Application.Interfaces.Services;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Abstractions.Services;
using SharedKernel.Commons.Bases;

namespace Engineering.Application.UseCases.StateChanges.Queries.GetReport;

public class GetStateChangeLogReportHandler(
    IUnitOfWork unitOfWork,
    IOrderingQuery orderingQuery)
    : IQueryHandler<GetStateChangeLogReportQuery, IEnumerable<StateChangeLogResponseDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IOrderingQuery _orderingQuery = orderingQuery;

    public async Task<BaseResponse<IEnumerable<StateChangeLogResponseDto>>> Handle(
        GetStateChangeLogReportQuery request,
        CancellationToken cancellationToken)
    {
        var response = new BaseResponse<IEnumerable<StateChangeLogResponseDto>>();

        try
        {
            // 1. Construir filtros para el repositorio
            var filter = new StateChangeLogFilterDto
            {
                Modulo = request.Modulo,
                Entidad = request.Entidad,
                IdRegistro = request.IdRegistro,
                IdProyecto = request.IdProyecto,
                IdTarea = request.IdTarea,
                Consecutivo = request.Consecutivo,
                EstadoAnterior = request.EstadoAnterior,
                EstadoNuevo = request.EstadoNuevo,
                Accion = request.Accion,
                Usuario = request.Usuario,
                FechaDesde = request.FechaDesde,
                FechaHasta = request.FechaHasta
            };

            // 2. Traer datos filtrados desde Dapper (sin orden ni paginación)
            var allLogs = (await _unitOfWork.LogMovement.GetByFilterAsync(filter)).ToList();

            // 3. Aplicar orden + paginación con IOrderingQuery (como Users)
            if (string.IsNullOrWhiteSpace(request.Sort))
            {
                request.Sort = "Fecha"; // propiedad de StateChangeLogResponseDto
                request.Order = "desc";
            }

            // Convertimos a IQueryable para que OrderingQuery funcione
            var queryable = allLogs.AsQueryable();
            var pagedQueryable = _orderingQuery.Ordering(request, queryable);

            var items = pagedQueryable.ToList();

            // 4. Armar respuesta
            response.IsSuccess = true;
            response.TotalRecords = allLogs.Count;
            response.Data = items;
            response.Message = "Consulta exitosa.";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }

        return response;
    }
}
