using MediatR;
using SharedKernel.Abstractions.Messaging;
using Country.Application.Dtos;

namespace Country.Application.UseCases
{
    public class GetCountryByIdQuery : IQuery<CountryDto>
    {
        public int Id { get; set; }
    }
}