using CourierJob.Application.Dtos;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
namespace CourierJob.Application.UseCases;

public class GetAllCourierJobQuery : BaseFilters, IQuery<IEnumerable<CourierJob.Application.Dtos.CourierJobDto>> {
    public new string? Sort { get; set; }


}