namespace ResultCouriers.Application.Dtos;

public class CreateResultCourierDto
{

    public int IdCourierJob { get; set; }
    public int IdCourier { get; set; }


    public decimal Price { get; set; }


    public string Currency { get; set; }
    public string Service { get; set; }
    public DateTime? Eta { get; set; }
}