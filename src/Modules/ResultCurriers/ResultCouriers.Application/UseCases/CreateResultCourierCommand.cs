using SharedKernel.Abstractions.Messaging;

public class CreateResultCourierCommand : ICommand<bool>
{
    public int Id { get; set; }
    public int IdCourierJob { get; set; }
    public int IdCourier { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; }
    public string Service { get; set; }
    public DateTime? Eta { get; set; }
}