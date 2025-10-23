using Country.Application.Interfaces;
using Country.Application.Dtos;
using Mapster;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Country.Application.UseCases;

public class GetCountryByIdQueryHandler : IQueryHandler<GetCountryByIdQuery,CountryDto>
{
    private readonly ICountryRepository _repository;

    public GetCountryByIdQueryHandler(ICountryRepository repository)
    {
        _repository = repository;
    }

    public async Task<BaseResponse<CountryDto>> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<CountryDto>();

        try
        {
            Domain.Entities.CountryEntity? user = await _repository.GetByIdAsync(request.Id);

            if (user is null)
            {
                response.IsSuccess = false;
                response.Message = "No se encontraron registros.";
                return response;
            }

            response.IsSuccess = true;
            response.Data = user.Adapt<CountryDto>();
            response.Message = "Consulta exitosa";
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}
