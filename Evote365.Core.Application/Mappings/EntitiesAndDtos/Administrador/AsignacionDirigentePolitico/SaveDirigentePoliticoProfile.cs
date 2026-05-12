using AutoMapper;
using Evote365.Core.Application.Dtos.Administradores.AsignacionDirigentePolitico;
using Evote366.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Mappings.EntitiesAndDtos.Administrador.AsignacionDirigentePolitico
{
    public class SaveDirigentePoliticoProfile : Profile
    {
        public SaveDirigentePoliticoProfile()
        {
            CreateMap<DirigentePartido, DirigentePartidoDto>()
    .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => src.Usuario.Nombre + " " + src.Usuario.Apellido))
    .ForMember(dest => dest.SiglasPartido, opt => opt.MapFrom(src => src.PartidoPolitico.Siglas));

            CreateMap<DirigentePartidoDto, DirigentePartido>()
                .ForMember(dest => dest.Usuario, opt => opt.Ignore())
                .ForMember(dest => dest.PartidoPolitico, opt => opt.Ignore());

            CreateMap<SaveAsignacionDto, DirigentePartido>();
        }

    }
}
