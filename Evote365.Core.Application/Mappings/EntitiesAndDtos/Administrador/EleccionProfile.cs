using AutoMapper;
using Evote365.Core.Application.Dtos.Administrador.Eleccion;
using Evote365.Core.Application.Dtos.Dirigente.AsignacionCandidatos;
using Evote366.Core.Domain.Entities;


namespace Evote365.Core.Application.Mappings.EntitiesAndDtos.Administrador
{
    public class EleccionProfile : Profile
    {
        public EleccionProfile()
        {
            // Elección → EleccionDto
            CreateMap<Eleccion, EleccionDto>()
                .ForMember(dest => dest.CantidadPartidos, opt => opt.Ignore())
                .ForMember(dest => dest.CantidadCandidatos, opt => opt.Ignore())
                .ForMember(dest => dest.CantidadPuestos, opt => opt.Ignore())
                .ForMember(dest => dest.TotalVotosEmitidos, opt => opt.Ignore());

            // Resultados por candidato
            CreateMap<Candidato, ResultadoCandidatoDto>()
                .ForMember(dest => dest.NombreCandidato, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.PartidoSiglas, opt => opt.MapFrom(src => src.PartidoPolitico.Siglas))
                .ForMember(dest => dest.VotosRecibidos, opt => opt.Ignore())     // Se calcula en servicio
                .ForMember(dest => dest.Porcentaje, opt => opt.Ignore());       // Se calcula en servicio

            // Resultado por puesto
            CreateMap<PuestoElectivo, ResultadoPuestoDto>()
                .ForMember(dest => dest.NombrePuesto, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.Candidatos, opt => opt.Ignore());       // Se llena en servicio

            // Resultado global
            CreateMap<Eleccion, ResultadoEleccionDto>()
                .ForMember(dest => dest.NombreEleccion, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.FechaRealizacion, opt => opt.MapFrom(src => src.FechaRealizacion))
                .ForMember(dest => dest.ResultadosPorPuesto, opt => opt.Ignore()); // Se llena en servicio

            CreateMap<PuestoElectivo, PuestoAsignableDto>()
            .ForMember(dest => dest.PuestoId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.NombrePuesto, opt => opt.MapFrom(src => src.Nombre))
            .ForMember(dest => dest.EstaAsignado, opt => opt.Ignore());

            CreateMap<AsignacionPuestosDto, List<EleccionPuesto>>()
                .ConvertUsing(dto => dto.PuestosSeleccionados
                    .Select(puestoId => new EleccionPuesto
                    {
                        EleccionId = dto.EleccionId,
                        PuestoElectivoId = puestoId
                    }).ToList());
        }
    }
}

