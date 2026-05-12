using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Dtos.Dirigente.AlianzaPolitica
{
    public class ConfirmarAccionAlianzaDto
    {
        public int SolicitudId { get; set; }

        public string NombrePartido { get; set; } = string.Empty;
        public string SiglasPartido { get; set; } = string.Empty;
    }
}
