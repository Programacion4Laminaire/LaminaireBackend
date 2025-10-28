using City.Application.Interfaces;
using City.Application.Dtos;
using Mapster;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using City.Domain.Entities;

namespace City.Application.UseCases;

public class GetCityByIdHandler(ICityRepository repository) : IQueryHandler<GetCityByIdQuery,CityDto>
{
    private readonly ICityRepository _repository =repository;
   

    public async Task<BaseResponse<CityDto>> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<CityDto>();

        try
        {
           CityEntity? user = await _repository.GetByIdAsync(request.Id);

            if (user is null)
            {
                response.IsSuccess = false;
                response.Message = "No se encontraron registros.";
                return response;
            }

            response.IsSuccess = true;
            response.Data = user.Adapt<CityDto>();
            response.Message = "Consulta exitosa";
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}
