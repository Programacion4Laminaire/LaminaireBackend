
using AutoMapper;
using Couriers.Application.Dtos;

namespace Couriers.Application.Mappings;
public class CourierMappingProfile : Profile
{
    public CourierMappingProfile()
    {
        // Mapeo desde la entidad de Dominio al DTO de respuesta
        CreateMap<SharedKernel.Domain.Entities.Couriers, CourierDto>();

        // Mapeo desde el DTO de creación a la entidad de Dominio
        CreateMap<CourierDto, SharedKernel.Domain.Entities.Couriers>();

        // Mapeo desde el DTO de actualización a la entidad de Dominio
        CreateMap<UpdateCourierDto, SharedKernel.Domain.Entities.Couriers>();
    }
}