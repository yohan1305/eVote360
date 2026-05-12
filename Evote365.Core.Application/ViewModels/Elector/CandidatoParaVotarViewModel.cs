using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.ViewModels.Elector
{
    public class CandidatoParaVotarViewModel
    {
        public int CandidatoId { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string PartidoNombre { get; set; } = string.Empty;
        public string PartidoSiglas { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public string? FotoUrl { get; set; }

        public int PartidoId { get; set; }

    }
}
