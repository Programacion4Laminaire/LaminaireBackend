using City.Application.Interfaces;
using MediatR;

public class DeleteCityCommandHandler : IRequestHandler<DeleteCityCommand, bool>
{
    private readonly ICityRepository _repository;

    public DeleteCityCommandHandler(ICityRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteCityCommand request, CancellationToken cancellationToken)
    {
        // Busca el país para asegurarte de que existe antes de eliminarlo
        var countryToDelete = await _repository.GetByIdAsync(request.Id);
        if (countryToDelete == null)
        {
            return false;
        }

        // Llama al repositorio para eliminar el país
        await _repository.DeleteAsync(countryToDelete.Id);

        return true;
    }
}