using City.Application.Dtos;
using City.Application.Interfaces;
using City.Domain.Entities;
using Mapster;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;

namespace Identity.Application.UseCases.Users.Commands.CreateCommand;

public class CreateCityCommandHandler(ICityRepository repository) : ICommandHandler<CreateCityCommand, bool>
{
    private readonly ICityRepository _repository = repository;

    public async Task<BaseResponse<bool>> Handle(CreateCityCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var city = request.Adapt<CityEntity>();
            await _repository.AddAsync(city);
            response.IsSuccess = true;
            response.Message = "Registro exitoso";
        }
        catch (Exception ex)
        {
            response.Message = ex.Message;
        }

        return response;
    }
}
