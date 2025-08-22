using SharedKernel.Commons.Bases;

namespace SharedKernel.Commons.Exceptions;

public class ValidationException : Exception
{
    // Lista de errores de validación
    public IEnumerable<BaseError>? Errors { get; }

    // Constructor sin parámetros, inicializa lista vacía
    public ValidationException() : base()
    {
        Errors = new List<BaseError>();
    }

    // Constructor que recibe errores y los asigna
    public ValidationException(IEnumerable<BaseError> errors) : this()
    {
        Errors = errors;
    }
}
