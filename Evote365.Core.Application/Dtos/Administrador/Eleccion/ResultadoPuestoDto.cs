using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Dtos.Administrador.Eleccion
{
    public class ResultadoPuestoDto
    {
        public string NombrePuesto { get; set; } = null!;
        public List<ResultadoCandidatoDto> Candidatos { get; set; } = new();
    }
}
