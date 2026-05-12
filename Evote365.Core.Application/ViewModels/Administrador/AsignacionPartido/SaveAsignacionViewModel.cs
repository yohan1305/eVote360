using System.ComponentModel.DataAnnotations;

namespace Evote365.Core.Application.ViewModels.Administrador.AsignacionPartido
{
   
    public class SaveAsignacionViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "Debe seleccionar un dirigente político.")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un partido político.")]
        public int PartidoPoliticoId { get; set; }

        public IEnumerable<OpcionItemViewModel>? DirigentesDisponibles { get; set; }
        public IEnumerable<OpcionItemViewModel>? PartidosDisponibles { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UsuarioId <= 0)
                yield return new ValidationResult("Debe seleccionar un dirigente político.", new[] { nameof(UsuarioId) });

            if (PartidoPoliticoId <= 0)
                yield return new ValidationResult("Debe seleccionar un partido político.", new[] { nameof(PartidoPoliticoId) });
        }
    }
}
