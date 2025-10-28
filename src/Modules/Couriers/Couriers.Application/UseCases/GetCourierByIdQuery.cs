using MediatR;
using SharedKernel.Abstractions.Messaging;
using Couriers.Application.Dtos;

namespace Couriers.Application.UseCases
{
    public class GetCourierByIdQuery : IQuery<CourierDto>
    {
        public int Id { get; set; }
    }
}