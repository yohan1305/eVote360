using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Dtos.Elector
{
    public class VotoItemDto
    {
        public required int PuestoId { get; set; }
        public required int CandidatoId { get; set; }
        public required int PartidoId { get; set; }
    }
}
