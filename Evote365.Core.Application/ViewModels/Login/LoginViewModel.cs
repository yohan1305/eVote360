using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.ViewModels.Login
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Debe ingresar su usuario.")]
        public string Usuario { get; set; }

        [Required(ErrorMessage = "Debe ingresar su contraseña.")]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; }
    }
}
