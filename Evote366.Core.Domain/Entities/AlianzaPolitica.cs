using Evote366.Core.Domain.Common.Enums;
namespace Evote366.Core.Domain.Entities
{
    public class AlianzaPolitica
    {
        public int Id { get; set; }

        public required int PartidoSolicitanteId { get; set; }
        public required int PartidoReceptorId { get; set; }

        public required DateTime FechaSolicitud { get; set; }
        public DateTime? FechaRespuesta { get; set; }

        public DateTime? FechaExpiracion { get; set; }

        public EstadoAlianza Estado { get; set; } = EstadoAlianza.EnEspera;

        // Navegación
        public PartidoPolitico? PartidoSolicitante { get; set; }
        public PartidoPolitico? PartidoReceptor { get; set; }
    }
}
