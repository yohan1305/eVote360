using System.Diagnostics;
using Evote365.Core.Application.Interfaces.Administrador;
using Evote365.Core.Application.Interfaces.Dirigente;
using Evote365.Core.Application.ViewModels.Home;
using Evote366.Core.Domain.Common.Enums;
using eVote365_2025.Models;
using Microsoft.AspNetCore.Mvc;

namespace eVote365_2025.Controllers.Administrador
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEleccionService _eleccionService;
        private readonly IPartidoPoliticoService _partidoPoliticoService;
        private readonly ICandidatoService _candidatoService;
        private readonly IAlianzaPoliticaService _alianzaPoliticaService;
        private readonly IAsignacionCandidatoPuestoService _asignacionCandidatoPuestoService;
        private readonly IUserSession _userSession;

        public HomeController(
            ILogger<HomeController> logger,
            IEleccionService eleccionService,
            IPartidoPoliticoService partidoPoliticoService,
            ICandidatoService candidatoService,
            IAlianzaPoliticaService alianzaPoliticaService,
            IAsignacionCandidatoPuestoService asignacionCandidatoPuestoService,
            IUserSession userSession)
        {
            _logger = logger;
            _eleccionService = eleccionService;
            _partidoPoliticoService = partidoPoliticoService;
            _candidatoService = candidatoService;
            _alianzaPoliticaService = alianzaPoliticaService;
            _asignacionCandidatoPuestoService = asignacionCandidatoPuestoService;
            _userSession = userSession;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Admin");
        }

        [ValidarRol("Administrador")]
        public async Task<IActionResult> Admin(int? anio)
        {
            var elecciones = await _eleccionService.GetAll();
            var aniosDisponibles = elecciones
                .Select(e => e.FechaRealizacion.Year)
                .Distinct()
                .OrderByDescending(y => y)
                .ToList();

            var anioSeleccionado = anio ?? aniosDisponibles.FirstOrDefault(DateTime.Today.Year);
            var resumenes = elecciones
                .Where(e => e.FechaRealizacion.Year == anioSeleccionado)
                .OrderByDescending(e => e.Estado == EstadoEleccion.EnProceso)
                .ThenByDescending(e => e.FechaRealizacion)
                .Select(e => new ResumenEleccionAdminViewModel
                {
                    Nombre = e.Nombre,
                    FechaRealizacion = e.FechaRealizacion,
                    Estado = e.Estado,
                    CantidadPartidos = e.CantidadPartidos,
                    CantidadCandidatos = e.CantidadCandidatos,
                    TotalVotosEmitidos = e.TotalVotosEmitidos
                })
                .ToList();

            return View(new AdminHomeViewModel
            {
                AniosDisponibles = aniosDisponibles,
                AnioSeleccionado = anioSeleccionado,
                Resumenes = resumenes
            });
        }

        [ValidarRol("DirigentePolitico")]
        public async Task<IActionResult> Dirigente()
        {
            var partidoId = _userSession.GetPartidoId();
            var partido = await _partidoPoliticoService.GetById(partidoId);
            if (partido is null)
            {
                return RedirectToAction("Index", "Login");
            }

            var candidatos = await _candidatoService.GetByPartidoDirigente();
            var alianzas = await _alianzaPoliticaService.GetAlianzasVigentesAsync(partidoId);
            var pendientes = await _alianzaPoliticaService.GetSolicitudesRecibidasAsync(partidoId);
            var asignaciones = await _asignacionCandidatoPuestoService.GetAsignacionesPorPartidoAsync(partidoId);

            return View(new DirigenteHomeViewModel
            {
                NombrePartido = partido.Nombre,
                SiglasPartido = partido.Siglas,
                LogoUrl = partido.LogoUrl,
                CandidatosActivos = candidatos.Count(c => c.Estado == EstadoEntidad.Activo),
                CandidatosInactivos = candidatos.Count(c => c.Estado == EstadoEntidad.Inactivo),
                CantidadAlianzasVigentes = alianzas.Count,
                CantidadSolicitudesPendientes = pendientes.Count,
                CandidatosAsignadosAPuesto = asignaciones.Select(a => a.CandidatoId).Distinct().Count()
            });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
