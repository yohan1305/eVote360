using AutoMapper;
using Evote365.Core.Application.Dtos.Administrador.Ciudadano;
using Evote365.Core.Application.ViewModels.Elector;
using Evote366.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Mappings.DtosAndViewModels.Elector
{
    public class ElectorViewModelProfile : Profile
    {
        public ElectorViewModelProfile()
        {
            CreateMap<CiudadanoDto, VerificarCedulaViewModel>()
                .ForMember(dest => dest.DocumentoIdentidad, opt => opt.MapFrom(src => src.DocumentoIdentidad));

            CreateMap<CiudadanoDto, ValidarIdentidadViewModel>()
                .ForMember(dest => dest.Cedula, opt => opt.MapFrom(src => src.DocumentoIdentidad));

            CreateMap<PuestoElectivo, PuestoResumenViewModel>()
                .ForMember(dest => dest.PuestoId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NombrePuesto, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.CantidadPartidos, opt => opt.Ignore())
                .ForMember(dest => dest.CantidadCandidatosReales, opt => opt.Ignore())
                .ForMember(dest => dest.YaVotado, opt => opt.Ignore());

            CreateMap<Candidato, CandidatoParaVotarViewModel>()
                .ForMember(dest => dest.CandidatoId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => $"{src.Nombre} {src.Apellido}"))
                .ForMember(dest => dest.PartidoId, opt => opt.MapFrom(src => src.PartidoPolitico.Id))
                .ForMember(dest => dest.PartidoNombre, opt => opt.MapFrom(src => src.PartidoPolitico.Nombre))
                .ForMember(dest => dest.PartidoSiglas, opt => opt.MapFrom(src => src.PartidoPolitico.Siglas))
                .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(src => src.PartidoPolitico.LogoUrl))
                .ForMember(dest => dest.FotoUrl, opt => opt.MapFrom(src => src.FotoUrl));

            CreateMap<AsignacionCandidatoPuesto, CandidatoParaVotarViewModel>()
                .ForMember(dest => dest.CandidatoId, opt => opt.MapFrom(src => src.CandidatoId))
                .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => $"{src.Candidato.Nombre} {src.Candidato.Apellido}"))
                .ForMember(dest => dest.FotoUrl, opt => opt.MapFrom(src => src.Candidato.FotoUrl))
                .ForMember(dest => dest.PartidoId, opt => opt.MapFrom(src => src.PartidoPoliticoId))
                .ForMember(dest => dest.PartidoNombre, opt => opt.MapFrom(src => src.PartidoPolitico.Nombre))
                .ForMember(dest => dest.PartidoSiglas, opt => opt.MapFrom(src => src.PartidoPolitico.Siglas))
                .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(src => src.PartidoPolitico.LogoUrl));
        }
    }
}
