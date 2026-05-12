using AutoMapper;
using Evote365.Core.Application.Dtos.Administrador.PuestoElectivoDto;
using Evote365.Core.Application.ViewModels.Administrador.PuestoElectivo;

namespace Evote365.Core.Application.Mappings.DtosAndViewModels.Administrador
{
    public class PuestoElectivoDtoMappingProfile : Profile
    {
        public PuestoElectivoDtoMappingProfile()
        {
            CreateMap<PuestoElectivoDto, PuestoElectivoViewModel>()
                .ReverseMap();

            CreateMap<PuestoElectivoDto, SavePuestoElectivoViewModel>()
                .ReverseMap();

            CreateMap<PuestoElectivoDto, DeletePuestoElectivoViewModel>()
                .ReverseMap();
        }

    }
}
