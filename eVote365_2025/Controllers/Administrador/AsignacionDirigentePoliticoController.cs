using AutoMapper;
using Evote365.Core.Application.Dtos.Administradores.AsignacionDirigentePolitico;
using Evote365.Core.Application.Interfaces.Administrador;
using Evote365.Core.Application.ViewModels.Administrador.AsignacionPartido;
using Microsoft.AspNetCore.Mvc;

namespace eVote365_2025.Controllers.Administrador
{
    public class AsignacionDirigentePoliticoController : Controller
    {
        private readonly IDirigentePartidoService _service;
        private readonly IUsuarioService _usuarioService;
        private readonly IPartidoPoliticoService _partidoService;
        private readonly IMapper _mapper;

        public AsignacionDirigentePoliticoController(
            IDirigentePartidoService service,
            IUsuarioService usuarioService,
            IPartidoPoliticoService partidoService,
            IMapper mapper)
        {
            _service = service;
            _usuarioService = usuarioService;
            _partidoService = partidoService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var relaciones = await _service.GetAllWithIncludes();
            var puedeModificar = await _service.CanModify();

            var vm = new AsignacionListadoViewModel
            {
                Relaciones = relaciones,
                PuedeModificar = puedeModificar
            };

            return View("Index", vm);
        }

        public async Task<IActionResult> Create()
        {
            var puedeModificar = await _service.CanModify();
            if (!puedeModificar)
                return RedirectToAction(nameof(Index));

            var usuarios = await _usuarioService.GetUsuariosActivosSinAsignacion();
            var partidos = await _partidoService.GetPartidosActivos();

            var vm = new SaveAsignacionViewModel
            {
                DirigentesDisponibles = usuarios.Select(u => new OpcionItemViewModel
                {
                    Value = u.Id,
                    Text = $"{u.Nombre} {u.Apellido}"
                }),
                PartidosDisponibles = partidos.Select(p => new OpcionItemViewModel
                {
                    Value = p.Id,
                    Text = p.Siglas
                })
            };

            return View("Create", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveAsignacionViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await CargarOpciones(vm);
                return View("Create", vm);
            }

            var yaAsignado = await _service.DirigenteYaAsignado(vm.UsuarioId);
            if (yaAsignado)
            {
                ModelState.AddModelError(nameof(vm.UsuarioId), "Este dirigente ya está asignado a un partido.");
                await CargarOpciones(vm);
                return View("Create", vm);
            }

            await _service.AsignarDirigenteAPartido(vm.UsuarioId, vm.PartidoPoliticoId);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ConfirmarDesvinculacion(int id)
        {
            var dto = await _service.GetById(id);
            if (dto == null)
                return RedirectToAction(nameof(Index));

            var vm = _mapper.Map<ConfirmarDesvinculacionViewModel>(dto);
            return View("ConfirmarDesvinculacion", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Desvincular(ConfirmarDesvinculacionViewModel vm)
        {
            await _service.DesvincularAsync(vm.RelacionId);
            return RedirectToAction(nameof(Index));
        }

        private async Task CargarOpciones(SaveAsignacionViewModel vm)
        {
            var usuarios = await _usuarioService.GetUsuariosActivosSinAsignacion();
            var partidos = await _partidoService.GetPartidosActivos();

            vm.DirigentesDisponibles = usuarios.Select(u => new OpcionItemViewModel
            {
                Value = u.Id,
                Text = $"{u.Nombre} {u.Apellido}"
            });

            vm.PartidosDisponibles = partidos.Select(p => new OpcionItemViewModel
            {
                Value = p.Id,
                Text = p.Siglas
            });
        }
    }
}