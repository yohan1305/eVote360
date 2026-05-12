using AutoMapper;
using Evote365.Core.Application.Dtos.Administrador.Usuario;
using Evote365.Core.Application.ViewModels.Administrador.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Mappings.DtosAndViewModels.Administrador
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<UsuarioDto, SaveUsuarioViewModel>().ReverseMap();
            CreateMap<UsuarioDto, UsuarioViewModel>().ReverseMap();
        }
    }
}
