using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.ViewModels.Administrador.PartidoPolitico
{
    public class SavePartidoPoliticoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public required string Nombre { get; set; }

        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "Las siglas son obligatorias")]
        public required string Siglas { get; set; }

        public IFormFile? LogoFile { get; set; }

        public string? LogoUrl { get; set; }
    }
}
