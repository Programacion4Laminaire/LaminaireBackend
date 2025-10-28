namespace City.Application.Mappings;
using AutoMapper;
using City.Application.Dtos;
using City.Domain.Entities;
using City.Application.Dtos; 


public class CityMappingProfile : Profile
{
    public CityMappingProfile()
    {
        // Mapeo desde la entidad de Dominio al DTO de respuesta
        CreateMap<CityEntity, CityDto>();

        // Mapeo desde el DTO de creación a la entidad de Dominio
        CreateMap<CityDto, CityEntity>();

        // Mapeo desde el DTO de actualización a la entidad de Dominio
        CreateMap<UpdateCityDto, CityEntity>();
    }
}