using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Dtos.Dirigente.Candidato
{
    public class SaveCandidatoDto
    {
        public int? Id { get; set; } // null en creación, valor en edición

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string Apellido { get; set; } = null!;

        public IFormFile? Foto { get; set; } // obligatoria en creación, opcional en edición

        public int? PuestoElectivoId { get; set; } // puede ser null
    }
}
