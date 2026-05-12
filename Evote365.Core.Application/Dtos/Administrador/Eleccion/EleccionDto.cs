using Evote366.Core.Domain.Common.Enums;

namespace Evote365.Core.Application.Dtos.Administrador.Eleccion
{
    public class EleccionDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public DateTime FechaRealizacion { get; set; }
        public EstadoEleccion Estado { get; set; }
        public int CantidadPartidos { get; set; }
        public int CantidadCandidatos { get; set; }
        public int CantidadPuestos { get; set; }
        public int TotalVotosEmitidos { get; set; }
    }
}
