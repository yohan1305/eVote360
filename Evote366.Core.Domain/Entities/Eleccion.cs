using Evote366.Core.Domain.Common.Enums;
namespace Evote366.Core.Domain.Entities
{
    public class Eleccion
    {
        public int Id { get; set; }

        public required string Nombre { get; set; }
        public required DateTime FechaRealizacion { get; set; }

        public EstadoEleccion Estado { get; set; } = EstadoEleccion.EnProceso;

        public ICollection<EleccionPuesto> PuestosEnEleccion { get; set; } = new List<EleccionPuesto>();
        public ICollection<Voto> VotosEmitidos { get; set; } = new List<Voto>();
    }
}
