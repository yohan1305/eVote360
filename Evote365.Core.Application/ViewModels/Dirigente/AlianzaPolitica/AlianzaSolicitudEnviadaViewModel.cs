using Evote366.Core.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.ViewModels.Dirigente.AlianzaPolitica
{
    public class AlianzaSolicitudEnviadaViewModel
    {
        public int Id { get; set; }

        public int PartidoReceptorId { get; set; }
        public string NombrePartidoReceptor { get; set; } = string.Empty;
        public string SiglasPartidoReceptor { get; set; } = string.Empty;

        public DateTime FechaSolicitud { get; set; }
        public EstadoAlianza Estado { get; set; }
    }
}
