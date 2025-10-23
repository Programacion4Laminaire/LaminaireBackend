using System.Text.Json.Serialization;

namespace SharedKernel.Domain.Entities;
public class ResultCouriers
{
   
    public int Id { get; set; }
    public int IdCourierJob { get; set; }
    public SharedKernel.Domain.Entities.CourierJob? CourierJob { get; set; }
    public int IdCourier { get; set; }

    public SharedKernel.Domain.Entities.Couriers? Couriers { get; set; }

    public decimal Price { get; set; }

   
    public string Currency { get; set; }
    public string Service { get; set; }
    public DateTime? Eta { get; set; } 
    public int Status { get; set; }

   public int Attempts {  get; set; }

}