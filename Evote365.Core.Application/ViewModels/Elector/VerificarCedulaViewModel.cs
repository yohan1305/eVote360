using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.ViewModels.Elector
{
    public class VerificarCedulaViewModel
    {
        [Required(ErrorMessage = "Debe ingresar su número de documento.")]
        [Display(Name = "Documento de Identidad")]
        public string DocumentoIdentidad { get; set; } = string.Empty;

        public string? MensajeError { get; set; }
    }
}
