using AutoMapper;
using Evote365.Core.Application.Dtos.Dirigente.Candidato;
using Evote366.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Mappings.EntitiesAndDtos.Dirigente
{
    public class CandidatoProfile : Profile
    {
        public CandidatoProfile()
        {

            CreateMap<Candidato, CandidatoDto>()
                .ForMember(dest => dest.PartidoSiglas, opt => opt.MapFrom(src => src.PartidoPolitico.Siglas))
                .ForMember(dest => dest.NombrePuesto, opt => opt.MapFrom(src =>
                    src.PuestoElectivo != null ? src.PuestoElectivo.Nombre : "Sin puesto asociado"));


            CreateMap<SaveCandidatoDto, Candidato>()
                .ForMember(dest => dest.FotoUrl, opt => opt.Ignore()) // se asigna manualmente en el servicio
                .ForMember(dest => dest.Id, opt => opt.Condition(src => src.Id.HasValue));


            CreateMap<Candidato, ConfirmarCambioEstadoCandidatoDto>()
                .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => $"{src.Nombre} {src.Apellido}"));

            CreateMap<Candidato, SaveCandidatoDto>();
        }
    }
}
