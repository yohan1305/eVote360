using AutoMapper;
using Evote365.Core.Application.Dtos.Administrador.Usuario;
using Evote365.Core.Application.Helpers;
using Evote365.Core.Application.Interfaces.Administrador;
using Evote365.Core.Application.ViewModels.Administrador.Usuario;
using Evote366.Core.Domain.Common.Enums;
using Microsoft.AspNetCore.Mvc;

namespace eVote365_2025.Controllers.Administrador
{
    [ValidarRol("Administrador")]
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;

        public UsuarioController(IUsuarioService usuarioService, IMapper mapper)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var usuarios = await _usuarioService.GetAll();
            var vm = _mapper.Map<List<UsuarioViewModel>>(usuarios);
            return View("Index", vm);
        }

        public async Task<IActionResult> Create()
        {
            if (!await _usuarioService.CanModify())
            {
                TempData["Error"] = "No se puede crear usuarios mientras haya una elección activa.";
                return RedirectToAction("Index");
            }

            return View("Save", new SaveUsuarioViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveUsuarioViewModel vm)
        {
            if (!await _usuarioService.CanModify())
                return RedirectToAction("Index");

            if (!ModelState.IsValid)
                return View("Save", vm);

            if (await _usuarioService.NombreUsuarioExistente(vm.NombreUsuario))
            {
                ModelState.AddModelError("NombreUsuario", "Ya existe un usuario con ese nombre.");
                return View("Save", vm);
            }

            var dto = _mapper.Map<UsuarioDto>(vm);
            var result = await _usuarioService.AddAsync(dto, vm.Password);

            if (result == null)
            {
                TempData["Error"] = "No se pudo crear el usuario.";
                return View("Save", vm);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!await _usuarioService.CanModify())
                return RedirectToAction("Index");

            var dto = await _usuarioService.GetById(id);
            if (dto == null)
                return RedirectToAction("Index");

            var vm = _mapper.Map<SaveUsuarioViewModel>(dto);
            return View("Save", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveUsuarioViewModel vm)
        {
            if (!await _usuarioService.CanModify())
                return RedirectToAction("Index");

            if (!ModelState.IsValid)
                return View("Save", vm);

            if (await _usuarioService.NombreUsuarioExistente(vm.NombreUsuario, vm.Id))
            {
                ModelState.AddModelError("NombreUsuario", "Ya existe un usuario con ese nombre.");
                return View("Save", vm);
            }

            // 🔒 Cargar datos originales para preservar campos críticos
            var original = await _usuarioService.GetByIdAsync(vm.Id);
            if (original == null)
            {
                TempData["Error"] = "No se encontró el usuario.";
                return View("Save", vm);
            }

            var dto = _mapper.Map<UsuarioDto>(vm);

            // 🔐 Preservar contraseña si no se está cambiando
            dto.PasswordHash = !string.IsNullOrWhiteSpace(vm.Password)
                ? PasswordEncryption.Hash(vm.Password)
                : original.PasswordHash;

            // 🔗 Preservar PartidoAsignadoId si no se envió
            dto.PartidoAsignadoId = vm.PartidoAsignadoId ?? original.PartidoAsignadoId;

            var result = await _usuarioService.UpdateAsync(dto, vm.Id);
            if (result == null)
            {
                TempData["Error"] = "No se pudo actualizar el usuario.";
                return View("Save", vm);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Activar(int id)
        {
            var dto = await _usuarioService.GetById(id);
            if (dto == null)
                return RedirectToAction("Index");

            var vm = _mapper.Map<UsuarioViewModel>(dto);
            return View("ConfirmarEstado", vm);
        }

        public async Task<IActionResult> Desactivar(int id)
        {
            var dto = await _usuarioService.GetById(id);
            if (dto == null)
                return RedirectToAction("Index");

            var vm = _mapper.Map<UsuarioViewModel>(dto);
            return View("ConfirmarEstado", vm);
        }

        public async Task<IActionResult> CambiarEstado(int id, EstadoEntidad nuevoEstado)
        {
            var result = await _usuarioService.CambiarEstado(id, nuevoEstado);
            if (!result)
            {
                TempData["Error"] = "No se pudo cambiar el estado del usuario.";
            }

            return RedirectToAction("Index");
        }
    }
}
