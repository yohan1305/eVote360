using System.ComponentModel.DataAnnotations;

namespace Evote365.Core.Application.ViewModels.Administrador.PuestoElectivo
{
    public class SavePuestoElectivoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Debes ingresar el nombre del puesto.")]
        public required string Nombre { get; set; }

        [Required(ErrorMessage = "Debes ingresar la descripción del puesto.")]
        public required string Descripcion { get; set; }
    }

}
