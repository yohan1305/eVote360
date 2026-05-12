using AutoMapper;
using Evote365.Core.Application.Interfaces.Administrador;
using Evote365.Core.Application.Interfaces.Dirigente;
using Evote365.Core.Application.ViewModels.Dirigente.AlianzaPolitica;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eVote365_2025.Controllers.Dirigente
{
  
    public class AlianzaPoliticaController : Controller
    {
        private readonly IAlianzaPoliticaService _alianzaService;
        private readonly IMapper _mapper;
        private readonly IUserSession _userSession;

        public AlianzaPoliticaController(
            IAlianzaPoliticaService alianzaService,
            IMapper mapper,
           IUserSession UserSession
        )
        {
            _alianzaService = alianzaService;
            _mapper = mapper;
            _userSession = UserSession;
        }

        // 🔹 Vista principal con los tres listados
        public async Task<IActionResult> Index()
        {
            int partidoId = _userSession.GetPartidoId();

            var recibidas = await _alianzaService.GetSolicitudesRecibidasAsync(partidoId);
            var enviadas = await _alianzaService.GetSolicitudesEnviadasAsync(partidoId);
            var vigentes = await _alianzaService.GetAlianzasVigentesAsync(partidoId);

            var vm = new AlianzasPoliticasIndexViewModel
            {
                SolicitudesRecibidas = _mapper.Map<List<AlianzaSolicitudRecibidaViewModel>>(recibidas),
                SolicitudesEnviadas = _mapper.Map<List<AlianzaSolicitudEnviadaViewModel>>(enviadas),
                AlianzasVigentes = _mapper.Map<List<AlianzaVigenteViewModel>>(vigentes)
            };

            return View(vm);
        }

        // 🔹 Crear nueva solicitud
        public async Task<IActionResult> Crear()
        {
            int partidoId =  _userSession.GetPartidoId();

            var disponibles = await _alianzaService.GetPartidosDisponiblesParaAlianza(partidoId);

            var vm = new CrearSolicitudAlianzaViewModel
            {
                PartidosDisponibles = disponibles
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CrearSolicitudAlianzaViewModel vm)
        {
            int partidoId =  _userSession.GetPartidoId();

            if (!ModelState.IsValid)
            {
                vm.PartidosDisponibles = await _alianzaService.GetPartidosDisponiblesParaAlianza(partidoId);
                return View(vm);
            }

            bool creado = await _alianzaService.CrearSolicitudAsync(partidoId, vm.PartidoReceptorId);
            if (!creado)
            {
                ModelState.AddModelError("", "No se pudo crear la solicitud. Verifique si hay una elección activa.");
                vm.PartidosDisponibles = await _alianzaService.GetPartidosDisponiblesParaAlianza(partidoId);
                return View(vm);
            }

            return RedirectToAction(nameof(Index));
        }

        // 🔹 Confirmar aceptar
        public async Task<IActionResult> ConfirmarAceptar(int id)
        {
            var dto = await _alianzaService.GetConfirmacionAceptarAsync(id);
            if (dto == null) return NotFound();

            var vm = _mapper.Map<ConfirmarAccionAlianzaViewModel>(dto);
            return View(vm);
        }
        

        [HttpPost]
        public async Task<IActionResult> ConfirmarAceptar(ConfirmarAccionAlianzaViewModel vm)
        {
            bool ok = await _alianzaService.AceptarSolicitudAsync(vm.SolicitudId);
            return RedirectToAction(nameof(Index));
        }

        // 🔹 Confirmar rechazar
        public async Task<IActionResult> ConfirmarRechazo(int id)
        {
            var dto = await _alianzaService.GetConfirmacionRechazoAsync(id);
            if (dto == null) return NotFound();

            var vm = _mapper.Map<ConfirmarAccionAlianzaViewModel>(dto);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarRechazo(ConfirmarAccionAlianzaViewModel vm)
        {
            bool ok = await _alianzaService.RechazarSolicitudAsync(vm.SolicitudId);
            return RedirectToAction(nameof(Index));
        }

        // 🔹 Confirmar eliminar
        public async Task<IActionResult> ConfirmarEliminar(int id)
        {
            var dto = await _alianzaService.GetConfirmacionEliminarAsync(id);
            if (dto == null) return NotFound();

            var vm = _mapper.Map<ConfirmarAccionAlianzaViewModel>(dto);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarEliminar(ConfirmarAccionAlianzaViewModel vm)
        {
            bool ok = await _alianzaService.EliminarSolicitudAsync(vm.SolicitudId);
            return RedirectToAction(nameof(Index));
        }
    }
}
