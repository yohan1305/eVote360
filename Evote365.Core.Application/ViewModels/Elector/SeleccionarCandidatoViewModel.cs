using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.ViewModels.Elector
{
    public class SeleccionarCandidatoViewModel
    {
        public int PuestoId { get; set; }
        public string NombrePuesto { get; set; } = string.Empty;
        public string CedulaElector { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar un candidato.")]
        public int? CandidatoIdSeleccionado { get; set; }

        public List<CandidatoParaVotarViewModel> Candidatos { get; set; } = new();

        public int? PartidoIdSeleccionado { get; set; }
        public string? MensajeError { get; set; }
    }
}
