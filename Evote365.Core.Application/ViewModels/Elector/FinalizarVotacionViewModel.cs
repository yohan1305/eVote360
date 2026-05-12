using Evote365.Core.Application.Dtos.Elector;

namespace Evote365.Core.Application.ViewModels.Elector
{
    public class FinalizarVotacionViewModel
    {
        public string CedulaElector { get; set; } = string.Empty;
        public List<string> PuestosFaltantes { get; set; } = [];
        public List<VotoResumenItemDto> ResumenVotos { get; set; } = [];
        public bool VotacionCompleta => PuestosFaltantes.Count == 0;
        public string? Mensaje { get; set; }
    }
}
