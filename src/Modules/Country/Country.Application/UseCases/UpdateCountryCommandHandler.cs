using Country.Application.Dtos;
using Country.Application.Interfaces;
using Country.Domain.Entities;
using Identity.Application.UseCases.Users.Commands.UpdateCommand;
using Mapster;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
using BC = BCrypt.Net.BCrypt;

namespace Country.Application.UseCases;

public class UpdateUserHandler(ICountryRepository repository) : ICommandHandler<UpdateCountryCommand, bool>
{
    private readonly ICountryRepository _repository = repository;

    public async Task<BaseResponse<bool>> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();

        try
        {
            var country = request.Adapt<UpdateCountryDto>();
     
           country.Name = request.Name;
           await _repository.UpdateAsync(request.Id,country);
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
