using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.ViewModels.Elector
{
    public class PuestoResumenViewModel
    {
        public int PuestoId { get; set; }
        public string NombrePuesto { get; set; } = string.Empty;
        public int CantidadPartidos { get; set; }
        public int CantidadCandidatosReales { get; set; }

        public bool YaVotado { get; set; }
    }
}
