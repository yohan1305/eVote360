using AutoMapper;
using Evote365.Core.Application.Dtos.Administrador.Usuario;
using Evote366.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Mappings.EntitiesAndDtos.Administrador
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<UsuarioDto, Usuario>().ReverseMap();
        }
    }

}
