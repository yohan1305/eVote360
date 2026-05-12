using AutoMapper;
using Evote365.Core.Application.Dtos.Dirigente.AsignacionCandidatos;
using Evote365.Core.Application.Interfaces.Administrador;
using Evote365.Core.Application.Interfaces.Dirigente;
using Evote365.Core.Application.ViewModels.Dirigente.Asignacion;
using Evote366.Core.Domain.Common.Enums;
using Microsoft.AspNetCore.Mvc;

namespace eVote365_2025.Controllers.Dirigente
{
    public class AsignacionCandidatoPuestoController : Controller
    {
        private readonly IAsignacionCandidatoPuestoService _asignacionService;
        private readonly IMapper _mapper;
        private readonly IUserSession _userSession;
        private readonly IEleccionService _eleccionService;

        public AsignacionCandidatoPuestoController(
            IAsignacionCandidatoPuestoService asignacionService,
            IMapper mapper,
            IUserSession userSession,
            IEleccionService eleccionService)
        {
            _asignacionService = asignacionService;
            _mapper = mapper;
            _userSession = userSession;
            _eleccionService = eleccionService;
        }

        private async Task<bool> EleccionActivaAsync()
        {
            var elecciones = await _eleccionService.GetAll();
            return elecciones.Any(e => e.Estado == EstadoEleccion.EnProceso);
        }

        //  Index: listado de relaciones Candidato - Puesto
        public async Task<IActionResult> Index()
        {
            var eleccionActiva = await EleccionActivaAsync();
            ViewBag.EleccionActiva = eleccionActiva;

            var partidoId = _userSession.GetPartidoId();
            var relacionesDto = await _asignacionService.GetAsignacionesPorPartidoAsync(partidoId);
            var vm = _mapper.Map<List<RelacionCandidatoPuestoViewModel>>(relacionesDto);

            foreach (var r in vm)
            {
                r.PuedeEliminar = !eleccionActiva;
            }

            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            if (await EleccionActivaAsync())
            {
                TempData["Error"] = "Operación denegada. La Elección ya está activa.";
                return RedirectToAction("Index");
            }

            var puestos = await _asignacionService.GetPuestosDisponiblesAsync();

            // Si no hay puestos libres en el partido, informamos de inmediato
            if (puestos == null || !puestos.Any())
            {
                var vmVacio = new AsignacionCandidatoPuestoViewModel
                {
                    PuestosDisponibles = new(),
                    CandidatosDisponibles = new(),
                    MensajeError = "Su partido ya ha cubierto todos los puestos electivos disponibles."
                };
                return View(vmVacio);
            }

            // Intentamos buscar candidatos para el primer puesto de la lista
            var puestoIdInicial = puestos.First().Id;
            var candidatos = await _asignacionService.GetCandidatosDisponiblesAsync(puestoIdInicial);

            var vm = new AsignacionCandidatoPuestoViewModel
            {
                PuestosDisponibles = puestos,
                CandidatosDisponibles = candidatos,
                PuestoIdSeleccionado = puestoIdInicial // Pre-seleccionamos el primero
            };

            // Si no hay candidatos para ese puesto específico, podemos avisar en la vista
            if (!candidatos.Any())
            {
                vm.MensajeError = $"No hay candidatos aptos (propios o aliados) para el puesto de {puestos.First().Nombre}.";
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AsignacionCandidatoPuestoViewModel vm)
        {
            // Método auxiliar para recargar las listas en caso de error
            async Task ReloadViewModelLists()
            {
                vm.PuestosDisponibles = await _asignacionService.GetPuestosDisponiblesAsync();
                // Recargamos candidatos basados en el puesto que el usuario seleccionó en el formulario
                vm.CandidatosDisponibles = await _asignacionService.GetCandidatosDisponiblesAsync(vm.PuestoIdSeleccionado);
            }

            if (await EleccionActivaAsync())
            {
                ModelState.AddModelError(string.Empty, "Operación denegada. La Elección ya está activa.");
                await ReloadViewModelLists();
                return View(vm);
            }

            if (!ModelState.IsValid)
            {
                await ReloadViewModelLists();
                return View(vm);
            }

            var dto = new AsignacionCandidatoPuestoDto
            {
                CandidatoId = vm.CandidatoIdSeleccionado,
                PuestoElectivoId = vm.PuestoIdSeleccionado
            };

            var resultado = await _asignacionService.AsignarAsync(dto);

            if (!resultado.Exito)
            {
                // El mensaje de error que viene del servicio (ej: "Este candidato ya aspira a otro puesto")
                vm.MensajeError = resultado.Mensaje;
                await ReloadViewModelLists();
                return View(vm);
            }

            TempData["Exito"] = resultado.Mensaje;
            return RedirectToAction("Index");
        }

        //AJAX: carga candidatos disponibles según puesto
        [HttpGet]
        public async Task<IActionResult> GetCandidatosDisponibles(int puestoId)
        {
            var list = await _asignacionService.GetCandidatosDisponiblesAsync(puestoId);
            return Json(list);
        }

        // onfirmarDesvinculacion GET
        public async Task<IActionResult> ConfirmarDesvinculacion(int id)
        {
            if (await EleccionActivaAsync())
            {
                TempData["Error"] = "Operación denegada. La Elección ya está activa.";
                return RedirectToAction("Index");
            }

            var partidoId = _userSession.GetPartidoId();
            var asignaciones = await _asignacionService.GetAsignacionesPorPartidoAsync(partidoId);
            var asignacion = asignaciones.FirstOrDefault(a => a.CandidatoId == id);

            if (asignacion == null)
                return NotFound();

            var vm = _mapper.Map<ConfirmarDesvinculacionViewModel>(asignacion);
            return View(vm);
        }

        //DeleteConfirmed POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int candidatoId)
        {
            if (await EleccionActivaAsync())
            {
                TempData["Error"] = "Operación denegada. La Elección ya está activa.";
                return RedirectToAction("Index");
            }

            var resultado = await _asignacionService.DesvincularAsync(candidatoId);
            if (!resultado.Exito)
            {
                TempData["Error"] = resultado.Mensaje;
                return RedirectToAction("Index");
            }

            TempData["Exito"] = resultado.Mensaje;
            return RedirectToAction("Index");
        }
    }
}