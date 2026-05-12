using AutoMapper;
using Evote365.Core.Application.Dtos.Administrador.PartidoPoliticoDto;
using Evote365.Core.Application.ViewModels.Administrador.PartidoPolitico;
using Evote366.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Mappings.DtosAndViewModels.Administrador
{
    public class PartidoPoliticoProfile : Profile
    {
        public PartidoPoliticoProfile()
        {
            // Entity ↔ DTO
            CreateMap<PartidoPolitico, PartidoPoliticoDto>().ReverseMap();

            // DTO ↔ ViewModel
            CreateMap<PartidoPoliticoDto, PartidoPoliticoViewModel>().ReverseMap();
            CreateMap<PartidoPoliticoDto, SavePartidoPoliticoViewModel>().ReverseMap();
            CreateMap<PartidoPoliticoDto, ToggleEstadoPartidoViewModel>().ReverseMap();
        }
    }
}
