using MediatR;

public class DeleteCourierJobCommand : IRequest<bool>
{
    public int Id { get; set; }
}
