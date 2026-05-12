using Evote366.Core.Domain.Common.Enums;

namespace Evote365.Core.Application.ViewModels.Home
{
    public class AdminHomeViewModel
    {
        public List<int> AniosDisponibles { get; set; } = [];
        public int AnioSeleccionado { get; set; }
        public List<ResumenEleccionAdminViewModel> Resumenes { get; set; } = [];
    }

    public class ResumenEleccionAdminViewModel
    {
        public string Nombre { get; set; } = string.Empty;
        public DateTime FechaRealizacion { get; set; }
        public EstadoEleccion Estado { get; set; }
        public int CantidadPartidos { get; set; }
        public int CantidadCandidatos { get; set; }
        public int TotalVotosEmitidos { get; set; }
    }
}
