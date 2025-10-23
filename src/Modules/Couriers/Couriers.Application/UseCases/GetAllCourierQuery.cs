using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
namespace Couriers.Application.UseCases;

public class GetAllCourierQuery : BaseFilters, IQuery<IEnumerable<Couriers.Application.Dtos.CourierDto>> {
    public new string? Sort { get; set; }

}