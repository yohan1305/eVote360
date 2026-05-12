using AutoMapper;
using Evote365.Core.Application.Dtos.Administrador.Ciudadano;
using Evote366.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Mappings.EntitiesAndDtos.Administrador
{
    public class CiudadanoProfile : Profile
    {
        public CiudadanoProfile()
        {
            CreateMap<Ciudadano, CiudadanoDto>().ReverseMap();
            CreateMap<CiudadanoDto, Ciudadano>()
    .ForMember(dest => dest.YaVoto, opt => opt.MapFrom(src => src.YaVoto))
    .ReverseMap();
        }
    }
}
