using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.ViewModels.Administrador.Eleccion
{
    public class CrearEleccionViewModel
    {
        [Required(ErrorMessage = "El nombre de la elección es obligatorio.")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "La fecha de realización es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaRealizacion { get; set; }

        public List<string> MensajesError { get; set; } = new();
    }
}
