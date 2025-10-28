using MediatR;

public class DeleteCityCommand : IRequest<bool>
{
    public int Id { get; set; }
}
