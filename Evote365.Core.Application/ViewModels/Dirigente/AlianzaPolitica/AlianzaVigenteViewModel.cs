using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.ViewModels.Dirigente.AlianzaPolitica
{
    public class AlianzaVigenteViewModel
    {
        public int Id { get; set; }

        public int PartidoAliadoId { get; set; }
        public string NombrePartidoAliado { get; set; } = string.Empty;
        public string SiglasPartidoAliado { get; set; } = string.Empty;

        public DateTime FechaAceptacion { get; set; }
    }
}
