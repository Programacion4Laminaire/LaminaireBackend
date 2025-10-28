using MediatR;
using SharedKernel.Abstractions.Messaging;
using City.Application.Dtos;

namespace City.Application.UseCases
{
    public class GetCityByIdQuery : IQuery<CityDto>
    {
        public int Id { get; set; }
    }
}