using MediatR;
using SharedKernel.Abstractions.Messaging;
using ResultCouriers.Application.Dtos;

namespace CourierJob.Application.UseCases
{
    public class GetResultCourierJobByIdQuery : IQuery<ResultCourierDto>
    {
        public int Id { get; set; }
    }
}