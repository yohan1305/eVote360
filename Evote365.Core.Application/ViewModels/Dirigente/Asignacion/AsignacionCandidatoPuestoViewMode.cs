using Evote365.Core.Application.Dtos.Dirigente.AsignacionCandidatos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.ViewModels.Dirigente.Asignacion
{
    public class AsignacionCandidatoPuestoViewModel
    {
        // Select de candidatos disponibles
        public List<CandidatoDisponibleDto> CandidatosDisponibles { get; set; } = new();

        // Select de puestos disponibles
        public List<PuestoDisponibleDto> PuestosDisponibles { get; set; } = new();

        // Valores seleccionados por el usuario
        [Required(ErrorMessage = "Debe seleccionar un candidato.")]
        public int CandidatoIdSeleccionado { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un puesto.")]
        public int PuestoIdSeleccionado { get; set; }

        // Mensaje de error personalizado (si lo necesitas en la vista)
        public string? MensajeError { get; set; }

      
    }
}
