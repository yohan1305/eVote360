using AutoMapper;
using Evote365.Core.Application.Dtos.Dirigente.Candidato;
using Evote365.Core.Application.ViewModels.Dirigente.Candidato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Mappings.DtosAndViewModels.Dirigente
{
    public class CandidatoViewModelProfile : Profile
    {
        public CandidatoViewModelProfile()
        {

            CreateMap<List<CandidatoDto>, CandidatoIndexViewModel>()
                .ForMember(dest => dest.Candidatos, opt => opt.MapFrom(src => src));

            
            CreateMap<SaveCandidatoDto, SaveCandidatoViewModel>()
                .ForMember(dest => dest.Candidato, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.PuestosDisponibles, opt => opt.Ignore()); // se llena en el controlador

           
            CreateMap<ConfirmarCambioEstadoCandidatoDto, ConfirmarCambioEstadoCandidatoViewModel>()
                .ForMember(dest => dest.Candidato, opt => opt.MapFrom(src => src));
        }
    }
}
