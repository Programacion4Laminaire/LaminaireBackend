using City.Application.Dtos;
using City.Application.Interfaces;
using City.Domain.Entities;
using City.Application.UseCases;
using Mapster;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;


namespace City.Application.UseCases;

public class UpdateUserHandler(ICityRepository repository) : ICommandHandler<UpdateCityCommand, bool>
{
    private readonly ICityRepository _repository = repository;

    public async Task<BaseResponse<bool>> Handle(UpdateCityCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var city = request.Adapt<UpdateCityDto>();
     
           city.Name = request.Name;
            city.CountryId = request.CountryId;
           await _repository.UpdateAsync(request.Id,city);
            response.IsSuccess = true;
            response.Message = "Actualización exitosa";
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}
