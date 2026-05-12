using Evote366.Core.Domain.Common.Enums;

namespace Evote365.Core.Application.ViewModels.Administrador.PuestoElectivo
{
    public class PuestoElectivoViewModel
    {
        public required int Id { get; set; }
        public required string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public required EstadoEntidad Estado { get; set; }

    }
}
