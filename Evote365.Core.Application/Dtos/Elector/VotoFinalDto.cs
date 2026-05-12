using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Core.Application.Dtos.Elector
{
    public class VotoFinalDto
    {
        public required string DocumentoIdentidad { get; set; }
        public required int EleccionId { get; set; }
        public DateTime FechaVoto { get; set; } = DateTime.UtcNow;
        public List<VotoItemDto> Votos { get; set; } = new();
        public string? Ip { get; set; }
        public string? UserAgent { get; set; }
    }
}
