using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace ResultCouriers.Application.UseCases;


public class GetResultCourierJobByIdCourierQuery : BaseFilters, IQuery<IEnumerable<ResultCouriers.Application.Dtos.ResultCourierDto>>
{

    public int IdCourier { get; }
    public new string? Sort { get; set; }

    public GetResultCourierJobByIdCourierQuery(int idCourier)
    {
        IdCourier = idCourier;
    }
}