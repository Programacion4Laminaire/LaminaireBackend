using ClosedXML.Excel;
using Logistics.Application.Dtos.AccessoryEquivalence;
using Logistics.Application.Interfaces.Services;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using System.Data;
using System.Drawing;

namespace Logistics.Application.UseCases.AccessoryEquivalence.Queries.ExportQuery;

public class ExportEquivalenceHandler(IUnitOfWork uow) : IQueryHandler<ExportEquivalenceQuery, Stream>
{
    private readonly IUnitOfWork _uow = uow;

    public async Task<BaseResponse<Stream>> Handle(ExportEquivalenceQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<Stream>();

        try
        {
            // Reutilizamos la lógica del repositorio para obtener los datos.
            // Pedimos un número muy grande de registros para exportar todo.
            const int maxRecords = int.MaxValue;
            const int numPage = 1;

            var (items, _) = await _uow.AccessoryEquivalence.GetPagedAsync(
                maxRecords, numPage, request.Sort, request.Order, request.NumFilter, request.TextFilter);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Equivalencias");

            // Definimos las cabeceras
            worksheet.Cell(1, 1).Value = "Código PT";
            worksheet.Cell(1, 2).Value = "Descripción PT";
            worksheet.Cell(1, 3).Value = "Código MP";
            worksheet.Cell(1, 4).Value = "Descripción MP";
            worksheet.Cell(1, 5).Value = "Costo";
            worksheet.Cell(1, 6).Value = "Fecha de Creación";

            // Aplicamos estilo a la cabecera
            var headerRange = worksheet.Range(1, 1, 1, 6);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;

            // Llenamos los datos
            int row = 2;
            foreach (var item in items)
            {
                worksheet.Cell(row, 1).Value = item.CodigoPT;
                worksheet.Cell(row, 2).Value = item.DescripcionPT;
                worksheet.Cell(row, 3).Value = item.CodigoMP;
                worksheet.Cell(row, 4).Value = item.DescripcionMP;
                worksheet.Cell(row, 5).Value = item.Costo;
                worksheet.Cell(row, 6).Value = item.FechaCreacion.ToString("yyyy-MM-dd HH:mm:ss");
                row++;
            }

            // Ajustamos el ancho de las columnas
            worksheet.Columns().AdjustToContents();

            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0; // Importante: resetear la posición del stream

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