using System.Linq;
using ClosedXML.Excel;
using Engineering.Application.Dtos.StateChanges;
using Engineering.Application.Interfaces.Services;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Engineering.Application.UseCases.StateChanges.Queries.ExportQuery;

public class ExportStateChangeLogHandler(IUnitOfWork uow)
    : IQueryHandler<ExportStateChangeLogQuery, Stream>
{
    private readonly IUnitOfWork _uow = uow;

    public async Task<BaseResponse<Stream>> Handle(
        ExportStateChangeLogQuery request,
        CancellationToken cancellationToken)
    {
        var response = new BaseResponse<Stream>();

        try
        {
            // 1. Armar el filtro (mismo contrato que el reporte)
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

            // 2. Traer datos filtrados desde el repositorio Dapper
            var queryable = (await _uow.LogMovement.GetByFilterAsync(filter))
                .AsQueryable();

            // 3. Aplicar orden 
            var sort = (request.Sort ?? "Fecha").Trim();
            var order = (request.Order ?? "desc").Trim().ToLowerInvariant();
            var asc = order == "asc";

            queryable = sort.ToLowerInvariant() switch
            {
                "modulo" => asc ? queryable.OrderBy(x => x.Modulo) : queryable.OrderByDescending(x => x.Modulo),
                "entidad" => asc ? queryable.OrderBy(x => x.Entidad) : queryable.OrderByDescending(x => x.Entidad),
                "idregistro" => asc ? queryable.OrderBy(x => x.IdRegistro) : queryable.OrderByDescending(x => x.IdRegistro),
                "idproyecto" => asc ? queryable.OrderBy(x => x.IdProyecto) : queryable.OrderByDescending(x => x.IdProyecto),
                "idtarea" => asc ? queryable.OrderBy(x => x.IdTarea) : queryable.OrderByDescending(x => x.IdTarea),
                "consecutivo" => asc ? queryable.OrderBy(x => x.Consecutivo) : queryable.OrderByDescending(x => x.Consecutivo),
                "accion" => asc ? queryable.OrderBy(x => x.Accion) : queryable.OrderByDescending(x => x.Accion),
                "usuario" => asc ? queryable.OrderBy(x => x.Usuario) : queryable.OrderByDescending(x => x.Usuario),
                "estadoanterior" => asc ? queryable.OrderBy(x => x.EstadoAnterior) : queryable.OrderByDescending(x => x.EstadoAnterior),
                "estadonuevo" => asc ? queryable.OrderBy(x => x.EstadoNuevo) : queryable.OrderByDescending(x => x.EstadoNuevo),
                _ => asc ? queryable.OrderBy(x => x.Fecha) : queryable.OrderByDescending(x => x.Fecha),
            };

            var items = queryable.ToList();

            // 4. Generar Excel 
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("CambiosEstado");

            // Cabeceras
            worksheet.Cell(1, 1).Value = "Fecha";
            worksheet.Cell(1, 2).Value = "Módulo";
            worksheet.Cell(1, 3).Value = "Entidad";
            worksheet.Cell(1, 4).Value = "Consecutivo";
            worksheet.Cell(1, 5).Value = "Proyecto";
            worksheet.Cell(1, 6).Value = "Tarea";
            worksheet.Cell(1, 7).Value = "Registro";
            worksheet.Cell(1, 8).Value = "Estado anterior";
            worksheet.Cell(1, 9).Value = "Estado nuevo";
            worksheet.Cell(1, 10).Value = "Acción";
            worksheet.Cell(1, 11).Value = "Usuario";
            worksheet.Cell(1, 12).Value = "Observaciones";

            var headerRange = worksheet.Range(1, 1, 1, 12);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

            // Datos
            var row = 2;
            foreach (var item in items)
            {
                worksheet.Cell(row, 1).Value = item.Fecha.ToString("yyyy-MM-dd HH:mm:ss");
                worksheet.Cell(row, 2).Value = item.Modulo;
                worksheet.Cell(row, 3).Value = item.Entidad;
                worksheet.Cell(row, 4).Value = item.Consecutivo;
                worksheet.Cell(row, 5).Value = item.IdProyecto;
                worksheet.Cell(row, 6).Value = item.IdTarea;
                worksheet.Cell(row, 7).Value = item.IdRegistro;
                worksheet.Cell(row, 8).Value = item.EstadoAnterior;
                worksheet.Cell(row, 9).Value = item.EstadoNuevo;
                worksheet.Cell(row, 10).Value = item.Accion;
                worksheet.Cell(row, 11).Value = item.Usuario;
                worksheet.Cell(row, 12).Value = item.Observaciones;
                row++;
            }

            worksheet.Columns().AdjustToContents();

            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            response.IsSuccess = true;
            response.Data = stream;
            response.Message = "Exportación exitosa.";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }

        return response;
    }
}
