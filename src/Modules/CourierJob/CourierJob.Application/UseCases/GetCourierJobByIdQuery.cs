using MediatR;
using SharedKernel.Abstractions.Messaging;
using CourierJob.Application.Dtos;

namespace CourierJob.Application.UseCases
{
    public class GetCourierJobByIdQuery : IQuery<CourierJobDto>
    {
        public int Id { get; set; }
    }
}