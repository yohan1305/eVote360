using AutoMapper;
using Evote365.Core.Application.Dtos.Dirigente.AlianzaPolitica;
using Evote365.Core.Application.ViewModels.Dirigente.AlianzaPolitica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Mappings.DtosAndViewModels.Dirigente
{
    public class AlianzaPoliticaViewModelProfile : Profile
    {
        public AlianzaPoliticaViewModelProfile()
        {
            //  Solicitudes recibidas
            CreateMap<AlianzaSolicitudRecibidaDto, AlianzaSolicitudRecibidaViewModel>();

            //  Solicitudes enviadas
            CreateMap<AlianzaSolicitudEnviadaDto, AlianzaSolicitudEnviadaViewModel>();

            //  Alianzas vigentes
            CreateMap<AlianzaVigenteDto, AlianzaVigenteViewModel>();

            //  Crear solicitud
            CreateMap<CrearSolicitudAlianzaDto, CrearSolicitudAlianzaViewModel>().ReverseMap();

            //  Confirmaciones
            CreateMap<ConfirmarAccionAlianzaDto, ConfirmarAccionAlianzaViewModel>();
        }
    }

}
