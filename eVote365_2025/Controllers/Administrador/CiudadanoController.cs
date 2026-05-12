using AutoMapper;
using Evote365.Core.Application.Dtos.Administrador.Ciudadano;
using Evote365.Core.Application.Interfaces.Administrador;
using Evote365.Core.Application.ViewModels.Administrador.Ciudadano;
using Evote366.Core.Domain.Common.Enums;
using Microsoft.AspNetCore.Mvc;

namespace eVote365_2025.Controllers.Administrador
{
    [ValidarRol("Administrador")]
    public class CiudadanoController : Controller
    {
        private readonly ICiudadanoService _ciudadanoService;
        private readonly IMapper _mapper;

        public CiudadanoController(ICiudadanoService ciudadanoService, IMapper mapper)
        {
            _ciudadanoService = ciudadanoService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var ciudadanos = await _ciudadanoService.GetAll();
            var vm = _mapper.Map<List<CiudadanoViewModel>>(ciudadanos);
            ViewBag.CanModify = await _ciudadanoService.CanModify();
            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            if (!await _ciudadanoService.CanModify()) return RedirectToAction("Index");
            return View("Save", new SaveCiudadanoViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(SaveCiudadanoViewModel vm)
        {
            if (!await _ciudadanoService.CanModify()) return RedirectToAction("Index");

            if (!ModelState.IsValid) return View("Save", vm);

            if (await _ciudadanoService.DocumentoIdentidadExistente(vm.DocumentoIdentidad))
            {
                ModelState.AddModelError("DocumentoIdentidad", "Ya existe un ciudadano con este documento.");
                return View("Save", vm);
            }

            var dto = _mapper.Map<CiudadanoDto>(vm);
            dto.YaVoto = false;
            await _ciudadanoService.AddAsync(dto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!await _ciudadanoService.CanModify()) return RedirectToAction("Index");

            var dto = await _ciudadanoService.GetById(id);
            if (dto == null) return RedirectToAction("Index");

            var vm = _mapper.Map<SaveCiudadanoViewModel>(dto);
            return View("Save", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SaveCiudadanoViewModel vm)
        {
            if (!await _ciudadanoService.CanModify()) return RedirectToAction("Index");

            if (!ModelState.IsValid) return View("Save", vm);

            if (await _ciudadanoService.DocumentoIdentidadExistente(vm.DocumentoIdentidad, vm.Id))
            {
                ModelState.AddModelError("DocumentoIdentidad", "Ya existe un ciudadano con este documento.");
                return View("Save", vm);
            }

            var dto = _mapper.Map<CiudadanoDto>(vm);
            await _ciudadanoService.UpdateAsync(dto, vm.Id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ToggleEstado(int id)
        {
            if (!await _ciudadanoService.CanModify()) return RedirectToAction("Index");

            var dto = await _ciudadanoService.GetById(id);
            if (dto == null) return RedirectToAction("Index");

            var vm = _mapper.Map<ToggleCiudadanoEstadoViewModel>(dto);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleEstadoConfirmed(int id)
        {
            var dto = await _ciudadanoService.GetById(id);
            if (dto == null) return RedirectToAction("Index");

            var nuevoEstado = dto.Estado == EstadoEntidad.Activo
                ? EstadoEntidad.Inactivo
                : EstadoEntidad.Activo;

            var ok = await _ciudadanoService.CambiarEstado(id, nuevoEstado);
            return RedirectToAction("Index");
        }
    }
}
