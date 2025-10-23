using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
namespace ResultCouriers.Application.UseCases;

public class GetAllResultCourierQuery : BaseFilters, IQuery<IEnumerable<ResultCouriers.Application.Dtos.ResultCourierDto>> {
    public new string? Sort { get; set; }
    public int Id { get; internal set; }
    public int IdCourier { get; internal set; }
}