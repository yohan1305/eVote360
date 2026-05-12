using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.ViewModels.Dirigente.AlianzaPolitica
{
    public class AlianzasPoliticasIndexViewModel
    {
        public List<AlianzaSolicitudRecibidaViewModel> SolicitudesRecibidas { get; set; } = new();
        public List<AlianzaSolicitudEnviadaViewModel> SolicitudesEnviadas { get; set; } = new();
        public List<AlianzaVigenteViewModel> AlianzasVigentes { get; set; } = new();
    }
}
