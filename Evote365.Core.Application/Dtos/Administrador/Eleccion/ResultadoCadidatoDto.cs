using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Dtos.Administrador.Eleccion
{

    public class ResultadoCandidatoDto
    {
        public string NombreCandidato { get; set; } = null!;
        public string PartidoSiglas { get; set; } = null!;
        public int VotosRecibidos { get; set; }
        public decimal Porcentaje { get; set; }
    }
}
