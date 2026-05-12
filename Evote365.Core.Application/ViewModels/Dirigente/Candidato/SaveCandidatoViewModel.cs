using Evote365.Core.Application.Dtos.Dirigente.Candidato;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Evote365.Core.Application.ViewModels.Dirigente.Candidato
{
    public class SaveCandidatoViewModel
    {
        public SaveCandidatoDto Candidato { get; set; } = new();

        public List<SelectListItem> PuestosDisponibles { get; set; } = new(); // para asignar puesto
        public bool EsEdicion => Candidato.Id.HasValue;
    }
}
