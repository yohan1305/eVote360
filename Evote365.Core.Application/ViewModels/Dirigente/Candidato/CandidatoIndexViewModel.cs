using Evote365.Core.Application.Dtos.Dirigente.Candidato;

namespace Evote365.Core.Application.ViewModels.Dirigente.Candidato
{
    public class CandidatoIndexViewModel
    {
        public List<CandidatoDto> Candidatos { get; set; } = new();
        public bool PuedeModificar { get; set; }
    }
}
