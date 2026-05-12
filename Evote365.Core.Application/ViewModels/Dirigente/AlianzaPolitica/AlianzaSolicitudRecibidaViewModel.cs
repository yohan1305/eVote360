using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.ViewModels.Dirigente.AlianzaPolitica
{
    public class AlianzaSolicitudRecibidaViewModel
    {
        public int Id { get; set; }

        public int PartidoSolicitanteId { get; set; }
        public string NombrePartidoSolicitante { get; set; } = string.Empty;
        public string SiglasPartidoSolicitante { get; set; } = string.Empty;

        public DateTime FechaSolicitud { get; set; }
    }
}
