using AutoMapper;
using Evote365.Core.Application.Dtos.Administrador.Eleccion;
using Evote365.Core.Application.Dtos.Dirigente.AsignacionCandidatos;
using Evote365.Core.Application.ViewModels.Administrador.Eleccion;
using Evote366.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Mappings.DtosAndViewModels.Administrador
{
    public class EleccionProfile : Profile
    {
        public EleccionProfile()
        {
            // DTO - ViewModel para resultados
            CreateMap<ResultadoCandidatoDto, ResultadoCandidatoViewModel>();
            CreateMap<ResultadoPuestoDto, ResultadoPuestoViewModel>();
            CreateMap<ResultadoEleccionDto, ResultadoEleccionViewModel>();
            CreateMap<SaveEleccionDto, Eleccion>();

            CreateMap<PuestoAsignableDto, PuestoAsignableViewModel>()
    .ForMember(dest => dest.PuestoId, opt => opt.MapFrom(src => src.PuestoId))
    .ForMember(dest => dest.NombrePuesto, opt => opt.MapFrom(src => src.NombrePuesto))
    .ForMember(dest => dest.Seleccionado, opt => opt.MapFrom(src => src.EstaAsignado));

            CreateMap<AsignacionPuestosViewModel, AsignacionPuestosDto>()
                .ForMember(dest => dest.EleccionId, opt => opt.MapFrom(src => src.EleccionId))
                .ForMember(dest => dest.PuestosSeleccionados, opt => opt.MapFrom(src =>
                    src.PuestosDisponibles
                        .Where(p => p.Seleccionado)
                        .Select(p => p.PuestoId)
                        .ToList()));


            // DTO - ViewModel para listado
            CreateMap<EleccionDto, ConfirmarFinalizacionViewModel>()
                .ForMember(dest => dest.EleccionId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NombreEleccion, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.FechaRealizacion, opt => opt.MapFrom(src => src.FechaRealizacion));
        }
    }
}
