using AutoMapper;
using Evote365.Core.Application.Dtos.Administrador.Eleccion;
using Evote365.Core.Application.Dtos.Dirigente.AsignacionCandidatos;
using Evote365.Core.Application.Dtos.Elector;
using Evote366.Core.Domain.Entities;

using CiudadanoDto = Evote365.Core.Application.Dtos.Administrador.Ciudadano.CiudadanoDto;

namespace Evote365.Core.Application.Mappings.EntitiesAndDtos.Elector
{
    public class ElectorProfile : Profile
    {
        public ElectorProfile()
        {
            // Ciudadano → CiudadanoDto
            CreateMap<Ciudadano, CiudadanoDto>();

            // CiudadanoDto → Ciudadano (opcional, útil si usas AddAsync o UpdateAsync)
            CreateMap<CiudadanoDto, Ciudadano>()
                .ForMember(dest => dest.VotosEmitidos, opt => opt.Ignore()); // no se mapea desde el DTO


            CreateMap<PuestoElectivo, PuestoResumenDto>()
            .ForMember(dest => dest.CantidadPartidos, opt => opt.MapFrom(src =>
                src.Asignaciones.Select(a => a.PartidoPoliticoId).Distinct().Count()))
                .ForMember(dest => dest.CantidadCandidatosReales, opt => opt.MapFrom(src =>
                src.Asignaciones.Select(a => a.CandidatoId).Distinct().Count()))
                .ForMember(dest => dest.YaVotado, opt => opt.Ignore()); // se calcula en servicio

            CreateMap<AsignacionCandidatoPuesto, CandidatoParaVotarDto>()
    .ForMember(dest => dest.CandidatoId, opt => opt.MapFrom(src => src.CandidatoId))
    .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src =>
        $"{src.Candidato.Nombre} {src.Candidato.Apellido}"))
    .ForMember(dest => dest.FotoUrl, opt => opt.MapFrom(src => src.Candidato.FotoUrl))
    .ForMember(dest => dest.PartidoId, opt => opt.MapFrom(src => src.PartidoPoliticoId))
    .ForMember(dest => dest.NombrePartido, opt => opt.MapFrom(src => src.PartidoPolitico.Nombre))
    .ForMember(dest => dest.LogoPartidoUrl, opt => opt.MapFrom(src => src.PartidoPolitico.LogoUrl));


            CreateMap<Voto, VotoItemDto>()
            .ForMember(dest => dest.PuestoId, opt => opt.MapFrom(src => src.PuestoElectivoId))
            .ForMember(dest => dest.CandidatoId, opt => opt.MapFrom(src => src.CandidatoId ?? 0))
            .ForMember(dest => dest.PartidoId, opt => opt.MapFrom(src =>
                src.Candidato != null ? src.Candidato.PartidoPoliticoId : 0));

            CreateMap<Eleccion, EleccionDto>()
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.FechaRealizacion, opt => opt.MapFrom(src => src.FechaRealizacion))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado));

        

        }
    }
}
