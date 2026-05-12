using AutoMapper;
using Evote365.Core.Application.Dtos.Administrador.Ciudadano;
using Evote365.Core.Application.ViewModels.Administrador.Ciudadano;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Mappings.DtosAndViewModels.Administrador
{
    public class CiudadanoViewModelProfile : Profile
    {
        public CiudadanoViewModelProfile()
        {
            CreateMap<CiudadanoDto, CiudadanoViewModel>().ReverseMap();
            CreateMap<CiudadanoDto, ToggleCiudadanoEstadoViewModel>().ReverseMap();
            CreateMap<CiudadanoDto, SaveCiudadanoViewModel>().ReverseMap(); // ✅ agregado
        }
    }
}
