using CourierJob.Application.Interfaces;
using MediatR;

public class DeleteCourierJobCommandHandler : IRequestHandler<DeleteCourierJobCommand, bool>
{
    private readonly ICourierJobRepository _repository;

    public DeleteCourierJobCommandHandler(ICourierJobRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteCourierJobCommand request, CancellationToken cancellationToken)
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