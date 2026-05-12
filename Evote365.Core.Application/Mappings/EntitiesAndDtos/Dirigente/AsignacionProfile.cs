using AutoMapper;
using Evote365.Core.Application.Dtos.Dirigente.AsignacionCandidatos;
using Evote366.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Mappings.EntitiesAndDtos.Dirigente
{
    public class AsignacionProfile : Profile
    {
        public AsignacionProfile()
        {
            // Candidato -> CandidatoDisponibleDto
            // Nota: YaTienePuesto y NombrePuestoActual deben ser poblados por el servicio antes del mapeo
            CreateMap<Candidato, CandidatoDisponibleDto>()
                .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => $"{src.Nombre} {src.Apellido}"))
                .ForMember(dest => dest.PartidoSiglas, opt => opt.MapFrom(src => src.PartidoPolitico != null ? src.PartidoPolitico.Siglas : string.Empty))
                .ForMember(dest => dest.PartidoNombre, opt => opt.MapFrom(src => src.PartidoPolitico != null ? src.PartidoPolitico.Nombre : string.Empty))
                // Evitar inferir YaTienePuesto desde la entidad; valor se establece en servicio
                .ForMember(dest => dest.YaTienePuesto, opt => opt.Ignore())
                .ForMember(dest => dest.NombrePuestoActual, opt => opt.Ignore())
                .ForMember(dest => dest.EsAliado, opt => opt.Ignore());

            // PuestoElectivo -> PuestoDisponibleDto (simple)
            CreateMap<PuestoElectivo, PuestoDisponibleDto>();

            // Candidato -> AsignacionListadoDto (usar solo si la entidad viene con includes)
            CreateMap<Candidato, AsignacionListadoDto>()
                .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => $"{src.Nombre} {src.Apellido}"))
                .ForMember(dest => dest.PartidoNombre, opt => opt.MapFrom(src => src.PartidoPolitico != null ? src.PartidoPolitico.Nombre : string.Empty))
                .ForMember(dest => dest.NombrePuesto, opt => opt.Ignore()) // setear en servicio desde asignaciones intermedias
                .ForMember(dest => dest.FechaAsignacion, opt => opt.Ignore())
                .ForMember(dest => dest.EsAliado, opt => opt.Ignore());

            // Eliminar/No mapear AsignacionCandidatoPuestoDto -> Candidato directamente
            // Si necesitas crear un Candidato desde DTO de asignación, hazlo explícitamente en el servicio.

            // Si necesitas mapear AsignacionCandidatoPuesto -> DTO, hacerlo explícito:
            CreateMap<AsignacionCandidatoPuesto, AsignacionCandidatoPuestoDto>()
                .ForMember(d => d.CandidatoId, o => o.MapFrom(s => s.CandidatoId))
                .ForMember(d => d.NombreCompleto, o => o.MapFrom(s => s.Candidato != null ? s.Candidato.Nombre + " " + s.Candidato.Apellido : string.Empty))
                .ForMember(d => d.PartidoSiglas, o => o.MapFrom(s => s.PartidoPolitico != null ? s.PartidoPolitico.Siglas : string.Empty))
                .ForMember(d => d.NombrePuesto, o => o.MapFrom(s => s.PuestoElectivo != null ? s.PuestoElectivo.Nombre : string.Empty))
                .ForMember(d => d.FechaAsignacion, o => o.MapFrom(s => s.FechaAsignacion));
        }
    }
}
