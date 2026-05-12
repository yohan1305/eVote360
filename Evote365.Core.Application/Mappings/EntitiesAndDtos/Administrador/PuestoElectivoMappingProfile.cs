using AutoMapper;
using Evote365.Core.Application.Dtos.Administrador.PuestoElectivoDto;
using Evote366.Core.Domain.Entities;

namespace Evote365.Core.Application.Mappings.EntitiesAndDtos.Administrador
{
    public class PuestoElectivoMappingProfile : Profile
    {
        public PuestoElectivoMappingProfile()
        {
            CreateMap<PuestoElectivo, PuestoElectivoDto>()
                .ReverseMap();
        }

    }
}
