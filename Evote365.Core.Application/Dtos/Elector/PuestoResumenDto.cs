using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Dtos.Elector
{
    public class PuestoResumenDto
    {
        public int PuestoId { get; set; }
        public string NombrePuesto { get; set; } = string.Empty;
        public int CantidadPartidos { get; set; }
        public int CantidadCandidatosReales { get; set; }
        public bool YaVotado { get; set; } = false; // útil para marcar visualmente
    }
}
