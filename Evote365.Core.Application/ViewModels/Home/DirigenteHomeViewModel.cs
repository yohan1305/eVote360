namespace Evote365.Core.Application.ViewModels.Home
{
    public class DirigenteHomeViewModel
    {
        public string NombrePartido { get; set; } = string.Empty;
        public string SiglasPartido { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public int CandidatosActivos { get; set; }
        public int CandidatosInactivos { get; set; }
        public int CantidadAlianzasVigentes { get; set; }
        public int CantidadSolicitudesPendientes { get; set; }
        public int CandidatosAsignadosAPuesto { get; set; }
    }
}
