using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Dtos.Elector
{
    public class CandidatoParaVotarDto
    {
        public int CandidatoId { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string? FotoUrl { get; set; }
        public int PartidoId { get; set; }
        public string NombrePartido { get; set; } = string.Empty;
        public string? LogoPartidoUrl { get; set; }
    }
}
