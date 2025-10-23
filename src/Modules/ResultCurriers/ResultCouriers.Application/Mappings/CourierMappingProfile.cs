
using AutoMapper;
using ResultCouriers.Application.Dtos;

namespace Couriers.Application.Mappings;
public class CourierMappingProfile : Profile
{
    public CourierMappingProfile()
    {
        // Mapeo desde la entidad de Dominio al DTO de respuesta
        CreateMap<ResultCouriers.Domain.Entities.ResultCouriers, ResultCourierDto>();

        // Mapeo desde el DTO de creación a la entidad de Dominio
        CreateMap<ResultCourierDto, ResultCouriers.Domain.Entities.ResultCouriers>();

      
    }
}