using Evote366.Core.Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Dtos.Administrador.PuestoElectivoDto
{
    public class PuestoElectivoDto
    {
        public required int Id { get; set; }
        public required string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public required EstadoEntidad Estado { get; set; }
    }

}
