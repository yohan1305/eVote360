using Evote366.Core.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.ViewModels.Administrador.Usuario
{
    public class SaveUsuarioViewModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio.")]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public string NombreUsuario { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "El rol es obligatorio.")]
        public RolUsuario Rol { get; set; }

        public int? PartidoAsignadoId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Id == 0) // Creación
            {
                if (string.IsNullOrWhiteSpace(Password))
                    yield return new ValidationResult("La contraseña es obligatoria.", new[] { nameof(Password) });

                if (string.IsNullOrWhiteSpace(ConfirmPassword))
                    yield return new ValidationResult("Debe confirmar la contraseña.", new[] { nameof(ConfirmPassword) });
            }
        }
    }
}
