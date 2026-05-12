using Evote365.Core.Application.Dtos.Dirigente.Candidato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.ViewModels.Dirigente.Candidato
{
    public class ConfirmarCambioEstadoCandidatoViewModel
    {
        public ConfirmarCambioEstadoCandidatoDto Candidato { get; set; } = new();

        public string Mensaje => Candidato.Activar
            ? "¿Está seguro que desea activar este candidato?"
            : "¿Está seguro que desea desactivar este candidato?";
    }
}
