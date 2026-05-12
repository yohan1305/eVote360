using AutoMapper;
using Evote365.Core.Application.Dtos.Dirigente.AsignacionCandidatos;
using Evote365.Core.Application.ViewModels.Dirigente.Asignacion;
using Evote366.Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Evote365.Core.Application.Mappings.DtosAndViewModels.Dirigente
{
    public class AsignacionViewModelProfile : Profile
    {
        public AsignacionViewModelProfile()
        {
            // CandidatoDisponibleDto → RelacionCandidatoPuestoViewModel
            CreateMap<CandidatoDisponibleDto, RelacionCandidatoPuestoViewModel>()
                .ForMember(dest => dest.CandidatoId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => src.NombreCompleto))
                .ForMember(dest => dest.PartidoSiglas, opt => opt.MapFrom(src => src.PartidoSiglas))
                .ForMember(dest => dest.NombrePuesto, opt => opt.MapFrom(src => src.NombrePuestoActual ?? "Sin puesto"))
                .ForMember(dest => dest.EsAliado, opt => opt.MapFrom(src => src.EsAliado))
                .ForMember(dest => dest.PuedeEliminar, opt => opt.Ignore());


            CreateMap<CandidatoDisponibleDto, SelectListItem>()
           .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id.ToString()))
           .ForMember(dest => dest.Text, opt => opt.MapFrom(src =>
               (src.YaTienePuesto
                   ? $"{src.NombreCompleto} - {src.PartidoSiglas} (Aspira a: {src.NombrePuestoActual})"
                   : $"{src.NombreCompleto} - {src.PartidoSiglas}")
               + (src.EsAliado ? " [Aliado]" : "")
           ));

            // PuestoDisponibleDto → SelectListItem (para select de puestos)
            CreateMap<PuestoDisponibleDto, SelectListItem>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Nombre));

            CreateMap<AsignacionCandidatoPuestoDto, ConfirmarDesvinculacionViewModel>()
    .ForMember(dest => dest.CandidatoId, opt => opt.MapFrom(src => src.CandidatoId))
    .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => src.NombreCompleto))
    .ForMember(dest => dest.NombrePuesto, opt => opt.MapFrom(src => src.NombrePuesto));

            CreateMap<CandidatoDisponibleDto, ConfirmarDesvinculacionViewModel>()
     .ForMember(d => d.CandidatoId, o => o.MapFrom(s => s.Id))
     .ForMember(d => d.NombreCompleto, o => o.MapFrom(s => s.NombreCompleto))
     .ForMember(d => d.NombrePuesto, o => o.MapFrom(s => s.NombrePuestoActual ?? "Sin puesto"))
     .ForMember(d => d.PartidoSiglas, o => o.MapFrom(s => s.PartidoSiglas))
     .ForMember(d => d.FechaAsignacion, o => o.Ignore()); // si quieres fecha, consíguela desde asignaciones/servicio

            //  AsignacionListadoDto → RelacionCandidatoPuestoViewModel (alternativa si usas este DTO en el listado)
            CreateMap<AsignacionListadoDto, RelacionCandidatoPuestoViewModel>()
                .ForMember(dest => dest.CandidatoId, opt => opt.MapFrom(src => src.CandidatoId))
                .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => src.NombreCompleto))
                .ForMember(dest => dest.PartidoSiglas, opt => opt.MapFrom(src => src.PartidoNombre)) // si no tienes siglas, usa nombre
                .ForMember(dest => dest.NombrePuesto, opt => opt.MapFrom(src => src.NombrePuesto))
                .ForMember(dest => dest.PuedeEliminar, opt => opt.Ignore());

            CreateMap<AsignacionCandidatoPuesto, AsignacionCandidatoPuestoDto>()
                .ForMember(d => d.CandidatoId, o => o.MapFrom(s => s.CandidatoId))
                .ForMember(d => d.NombreCompleto, o => o.MapFrom(s => s.Candidato.Nombre + " " + s.Candidato.Apellido))
                .ForMember(d => d.PartidoSiglas, o => o.MapFrom(s => s.PartidoPolitico.Siglas))
                .ForMember(d => d.NombrePuesto, o => o.MapFrom(s => s.PuestoElectivo.Nombre))
                .ForMember(d => d.FechaAsignacion, o => o.MapFrom(s => s.FechaAsignacion));


            CreateMap<AsignacionCandidatoPuestoDto, RelacionCandidatoPuestoViewModel>()
    .ForMember(dest => dest.CandidatoId, opt => opt.MapFrom(src => src.CandidatoId))
    .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => src.NombreCompleto))
    .ForMember(dest => dest.PartidoSiglas, opt => opt.MapFrom(src => src.PartidoSiglas))
    .ForMember(dest => dest.NombrePuesto, opt => opt.MapFrom(src => src.NombrePuesto))
    .ForMember(dest => dest.PuedeEliminar, opt => opt.Ignore()); // se asigna en el controlador
        }
    }

}
