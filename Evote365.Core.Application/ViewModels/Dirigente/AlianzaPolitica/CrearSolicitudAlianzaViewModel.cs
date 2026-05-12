using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.ViewModels.Dirigente.AlianzaPolitica
{
    public class CrearSolicitudAlianzaViewModel
    {
        [Required(ErrorMessage = "Debe seleccionar un partido.")]
        [Display(Name = "Nombre del partido")]
        public int PartidoReceptorId { get; set; }

        public List<OpcionItemViewModel> PartidosDisponibles { get; set; } = new();
    }
}
