using ClosedXML.Excel;
using SGI.Application.Dtos.Consumption;
using SGI.Application.Interfaces.Services;

namespace SGI.Infrastructure.Services;

public class ConsumptionExportService : IConsumptionExportService
{
    public byte[] ExportToExcel(IEnumerable<ConsumptionResponseDto> consumptions)
    {
        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add("Consumos");

        // ✅ Cabecera limpia y actualizada
        ws.Cell(1, 1).Value = "Recurso";
        ws.Cell(1, 2).Value = "Valor";
        ws.Cell(1, 3).Value = "Unidad";
        ws.Cell(1, 4).Value = "Fecha de lectura";
        ws.Cell(1, 5).Value = "Notas";
        ws.Cell(1, 6).Value = "Fecha creación";

        // ✅ Datos
        var row = 2;
        foreach (var c in consumptions)
        {
            ws.Cell(row, 1).Value = c.ResourceType;
            ws.Cell(row, 2).Value = c.Value;
            ws.Cell(row, 3).Value = c.Unit;
            ws.Cell(row, 4).Value = c.ReadingDate?.ToString("yyyy-MM-dd");
            ws.Cell(row, 5).Value = c.Note;
            ws.Cell(row, 6).Value = c.AuditCreateDate.ToString("yyyy-MM-dd HH:mm:ss");
            row++;
        }

        ws.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
