using AutoMapper;
using Evote365.Core.Application.Dtos.Administrador.PartidoPoliticoDto;
using Evote366.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Mappings.EntitiesAndDtos.Administrador
{
    public class PartidoPoliticoProfile : Profile
    {
        public PartidoPoliticoProfile()
        {
            // Entity → DTO
            CreateMap<PartidoPolitico, PartidoPoliticoDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
                .ForMember(dest => dest.Siglas, opt => opt.MapFrom(src => src.Siglas))
                .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(src => src.LogoUrl))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado));
        }
    }
}
