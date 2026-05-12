using AutoMapper;
using Evote365.Core.Application.Dtos.Dirigente.AlianzaPolitica;
using Evote366.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Mappings.DtosAndViewModels.Dirigente
{
    public class AlianzaPoliticaProfile : Profile
    {
        public AlianzaPoliticaProfile()
        {
            //  Solicitudes recibidas
            CreateMap<AlianzaPolitica, AlianzaSolicitudRecibidaDto>()
                .ForMember(dest => dest.PartidoSolicitanteId, opt => opt.MapFrom(src => src.PartidoSolicitanteId))
                .ForMember(dest => dest.NombrePartidoSolicitante, opt => opt.MapFrom(src => src.PartidoSolicitante.Nombre))
                .ForMember(dest => dest.SiglasPartidoSolicitante, opt => opt.MapFrom(src => src.PartidoSolicitante.Siglas))
                .ForMember(dest => dest.FechaSolicitud, opt => opt.MapFrom(src => src.FechaSolicitud));

            //  Solicitudes enviadas
            CreateMap<AlianzaPolitica, AlianzaSolicitudEnviadaDto>()
                .ForMember(dest => dest.PartidoReceptorId, opt => opt.MapFrom(src => src.PartidoReceptorId))
                .ForMember(dest => dest.NombrePartidoReceptor, opt => opt.MapFrom(src => src.PartidoReceptor.Nombre))
                .ForMember(dest => dest.SiglasPartidoReceptor, opt => opt.MapFrom(src => src.PartidoReceptor.Siglas))
                .ForMember(dest => dest.FechaSolicitud, opt => opt.MapFrom(src => src.FechaSolicitud))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado));

            //  Alianzas vigentes
            CreateMap<AlianzaPolitica, AlianzaVigenteDto>()
                .ForMember(dest => dest.PartidoAliadoId, opt => opt.MapFrom(src =>
                    src.PartidoSolicitanteId == src.PartidoReceptorId ? src.PartidoSolicitanteId :
                    src.PartidoSolicitanteId != src.PartidoReceptorId ?
                        (src.PartidoSolicitanteId == src.PartidoReceptorId ? src.PartidoReceptorId : src.PartidoSolicitanteId) : 0)
                )
                .ForMember(dest => dest.NombrePartidoAliado, opt => opt.MapFrom(src =>
                    src.PartidoSolicitanteId == src.PartidoReceptorId ? src.PartidoSolicitante.Nombre :
                    src.PartidoSolicitanteId != src.PartidoReceptorId ?
                        (src.PartidoSolicitanteId == src.PartidoReceptorId ? src.PartidoReceptor.Nombre : src.PartidoSolicitante.Nombre) : string.Empty)
                )
                .ForMember(dest => dest.SiglasPartidoAliado, opt => opt.MapFrom(src =>
                    src.PartidoSolicitanteId == src.PartidoReceptorId ? src.PartidoSolicitante.Siglas :
                    src.PartidoSolicitanteId != src.PartidoReceptorId ?
                        (src.PartidoSolicitanteId == src.PartidoReceptorId ? src.PartidoReceptor.Siglas : src.PartidoSolicitante.Siglas) : string.Empty)
                )
                .ForMember(dest => dest.FechaAceptacion, opt => opt.MapFrom(src => src.FechaRespuesta.Value));

            //  Confirmaciones (Aceptar, Rechazar, Eliminar)
            CreateMap<AlianzaPolitica, ConfirmarAccionAlianzaDto>()
                .ForMember(dest => dest.SolicitudId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NombrePartido, opt => opt.MapFrom(src => src.PartidoSolicitante.Nombre))
                .ForMember(dest => dest.SiglasPartido, opt => opt.MapFrom(src => src.PartidoSolicitante.Siglas));
        }
    }
}
