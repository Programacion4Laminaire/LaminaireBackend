using Country.Application.Dtos;
using Country.Application.Interfaces;
using Country.Domain.Entities;
using Mapster;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using BC = BCrypt.Net.BCrypt;

namespace Identity.Application.UseCases.Users.Commands.CreateCommand;

public class CreateUserHandler(ICountryRepository repository) : ICommandHandler<CreateCountryCommand, bool>
{
    private readonly ICountryRepository _repository = repository;

    public async Task<BaseResponse<bool>> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var country = request.Adapt<CountryEntity>();
            await _repository.AddAsync(country);
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
