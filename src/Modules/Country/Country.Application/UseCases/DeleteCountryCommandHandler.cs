
using System.Net;
using MediatR;
using SharedKernel.Abstractions.Messaging;
using SharedKernel.Commons.Bases;
namespace Country.Application.Interfaces;
public class DeleteCountryCommandHandler : ICommandHandler<DeleteCountryCommand, bool>
{
    private readonly ICountryRepository _repository;

    public DeleteCountryCommandHandler(ICountryRepository repository)
    {
        _repository = repository;
    }

    public async Task<BaseResponse<bool>> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<bool>();
        var countryToDelete = await _repository.GetByIdAsync(request.Id);
        if (countryToDelete == null)
        {
            response.IsSuccess = false;
            response.Message = "El Pais no existe en la base de datos.";
            return response;
        }

        // Llama al repositorio para eliminar el país
      
        try
        {
            await _repository.DeleteAsync(countryToDelete.Id);
            response.IsSuccess = true;
            response.Message = "Se elimino registro exitosamente";
          
        }
        catch
        (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = "No se pudo Eliminar";
        }
          return response;
        
    }

    
}