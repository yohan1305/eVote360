using AutoMapper;
using Evote365.Core.Application.Dtos.Administradores.AsignacionDirigentePolitico;
using Evote365.Core.Application.ViewModels.Administrador.AsignacionPartido;

namespace Evote365.Core.Application.Mappings.DtosAndViewModels.Administrador
{
    public class AsignacionViewModelProfile : Profile
    {
        public AsignacionViewModelProfile()
        {
            CreateMap<SaveAsignacionViewModel, SaveAsignacionDto>();
            CreateMap<SaveAsignacionDto, SaveAsignacionViewModel>();

            CreateMap<DirigentePartidoDto, ConfirmarDesvinculacionViewModel>()
                .ForMember(dest => dest.RelacionId, opt => opt.MapFrom(src => src.Id));

            CreateMap<SaveAsignacionDto, DirigentePartidoDto>(); //  NUEVO

        }
    }
}
