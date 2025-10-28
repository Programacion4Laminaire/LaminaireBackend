
using AutoMapper;
using CourierJob.Application.Dtos;

namespace CourierJob.Application.Mappings;
public class CourierJobMappingProfile : Profile
{
    public CourierJobMappingProfile()
    {
        // Mapeo desde la entidad de Dominio al DTO de respuesta
        CreateMap<SharedKernel.Domain.Entities.CourierJob, CourierJobDto>();

        // Mapeo desde el DTO de creación a la entidad de Dominio
        CreateMap<CourierJobDto, SharedKernel.Domain.Entities.CourierJob>();

        // Mapeo desde el DTO de actualización a la entidad de Dominio
        CreateMap<UpdateCourierJobDto, SharedKernel.Domain.Entities.CourierJob>();
    }
}