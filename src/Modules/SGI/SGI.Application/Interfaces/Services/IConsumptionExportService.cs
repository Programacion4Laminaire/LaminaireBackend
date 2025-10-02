using SGI.Application.Dtos.Consumption;

namespace SGI.Application.Interfaces.Services
{
    public interface IConsumptionExportService
    {
        byte[] ExportToExcel(IEnumerable<ConsumptionResponseDto> consumptions);
    }
}
