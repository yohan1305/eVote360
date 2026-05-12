using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Dtos.Dirigente.AlianzaPolitica
{
    public class AlianzaVigenteDto
    {
        public int Id { get; set; }

        public int PartidoAliadoId { get; set; }
        public string NombrePartidoAliado { get; set; } = string.Empty;
        public string SiglasPartidoAliado { get; set; } = string.Empty;

        public DateTime FechaAceptacion { get; set; }

        public DateTime? FechaExpiracion { get; set; }
    }
}
