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

        // ✅ CABECERA ACTUALIZADA
        ws.Cell(1, 1).Value = "Sede";
        ws.Cell(1, 2).Value = "Recurso";
        ws.Cell(1, 3).Value = "Lectura";
        ws.Cell(1, 4).Value = "Consumo diario (kWh / m³)";
        ws.Cell(1, 5).Value = "Unidad";
        ws.Cell(1, 6).Value = "Fecha de lectura";
        ws.Cell(1, 7).Value = "Notas";
        ws.Cell(1, 8).Value = "Fecha creación";

        // ✅ DATOS
        var row = 2;
        foreach (var c in consumptions)
        {
            ws.Cell(row, 1).Value = c.Sede;
            ws.Cell(row, 2).Value = c.ResourceType;
            ws.Cell(row, 3).Value = c.Value;                 // Lectura actual
            ws.Cell(row, 4).Value = c.DailyConsumption ?? 0; // Consumo diario calculado
            ws.Cell(row, 5).Value = c.Unit;
            ws.Cell(row, 6).Value = c.ReadingDate?.ToString("yyyy-MM-dd");
            ws.Cell(row, 7).Value = c.Note;
            ws.Cell(row, 8).Value = c.AuditCreateDate.ToString("yyyy-MM-dd HH:mm:ss");
            row++;
        }

        // ✅ Estilos
        ws.Row(1).Style.Font.Bold = true;
        ws.Row(1).Style.Fill.BackgroundColor = XLColor.FromArgb(217, 225, 242);
        ws.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
