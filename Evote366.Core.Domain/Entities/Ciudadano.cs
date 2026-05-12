using Evote366.Core.Domain.Common.Enums;
namespace Evote366.Core.Domain.Entities
{
    public class Ciudadano
    {
        public int Id { get; set; }

        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public string? Email { get; set; }
        public required string DocumentoIdentidad { get; set; }

        public EstadoEntidad Estado { get; set; } = EstadoEntidad.Activo;

        public bool YaVoto { get; set; } = false;

        public ICollection<Voto> VotosEmitidos { get; set; } = new List<Voto>();

    }
}
