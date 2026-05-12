using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Dtos.Dirigente.AlianzaPolitica
{
    public class AlianzaSolicitudRecibidaDto
    {
        public int Id { get; set; }

        public int PartidoSolicitanteId { get; set; }
        public string NombrePartidoSolicitante { get; set; } = string.Empty;
        public string SiglasPartidoSolicitante { get; set; } = string.Empty;

        public DateTime FechaSolicitud { get; set; }
    }
}
