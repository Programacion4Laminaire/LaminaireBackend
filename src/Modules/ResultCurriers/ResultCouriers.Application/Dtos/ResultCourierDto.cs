using Couriers.Application.Dtos;
using SharedKernel.Domain.Entities;

namespace ResultCouriers.Application.Dtos;

public class ResultCourierDto
{

    public int Id {  get; set; }
    public int IdCourierJob { get; set; }
    public int IdCourier { get; set; }

    bool IsActive { get; set; }

    public decimal Price { get; set; }


    public string Currency { get; set; }
    public string Service { get; set; }
    public DateTime? Eta { get; set; }
    
    public int Status { get; set; }

    public int? idCouriers{get; set;}
    public string? CouriersName { get; set; }
    public string? CouriersURL { get; set; }

    public string? CouriersUsername { get; set; }

    public string? CouriersPassword { get; set; }

    public bool? CouriersIsactive { get; set; }
    public string ciudad { get; set; }

}