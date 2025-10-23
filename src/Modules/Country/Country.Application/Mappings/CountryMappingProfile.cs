namespace Country.Application.Mappings;
using AutoMapper;
using Country.Application.Dtos; // Referencia a la carpeta de DTOs
using Country.Domain.Entities;

public class CountryMappingProfile : Profile
{
    public CountryMappingProfile()
    {
        // Mapeo desde la entidad de Dominio al DTO de respuesta
        CreateMap<CountryEntity,CountryDto>();

        // Mapeo desde el DTO de creación a la entidad de Dominio
        CreateMap<CreateCountryDto,CountryEntity>();

        // Mapeo desde el DTO de actualización a la entidad de Dominio
        CreateMap<UpdateCountryDto,CountryEntity>();
    }
}