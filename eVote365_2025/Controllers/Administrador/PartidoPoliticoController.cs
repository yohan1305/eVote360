using AutoMapper;
using Evote365.Core.Application.Dtos.Administrador.PartidoPoliticoDto;
using Evote365.Core.Application.Interfaces.Administrador;
using Evote365.Core.Application.ViewModels.Administrador.PartidoPolitico;
using Microsoft.AspNetCore.Mvc;

namespace eVote365_2025.Controllers.Administrador
{
    [ValidarRol("Administrador")]
    public class PartidoPoliticoController : Controller
    {
        private readonly IPartidoPoliticoService _partidoService;
        private readonly IMapper _mapper;

        public PartidoPoliticoController(IPartidoPoliticoService partidoService, IMapper mapper)
        {
            _partidoService = partidoService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var partidos = await _partidoService.GetAll();
            var vms = _mapper.Map<List<PartidoPoliticoViewModel>>(partidos);
            ViewBag.CanModify = await _partidoService.CanModify();
            return View(vms);
        }

        public async Task<IActionResult> Create()
        {
            if (!await _partidoService.CanModify()) return RedirectToAction("Index");

            return View("Save", new SavePartidoPoliticoViewModel
            {
                Nombre = "",
                Siglas = ""
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(SavePartidoPoliticoViewModel vm)
        {
            if (!await _partidoService.CanModify()) return RedirectToAction("Index");

            if (!ModelState.IsValid) return View("Save", vm);

            if (await _partidoService.SiglasExistente(vm.Siglas))
            {
                ModelState.AddModelError("Siglas", "Ya existe un partido con estas siglas.");
                return View("Save", vm);
            }

            var dto = _mapper.Map<PartidoPoliticoDto>(vm);

            if (vm.LogoFile != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(vm.LogoFile.FileName);
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img");

                // Crear carpeta si no existe
                Directory.CreateDirectory(folderPath);

                var fullPath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await vm.LogoFile.CopyToAsync(stream);
                }

                dto.LogoUrl = "/img/" + fileName;
            }

            await _partidoService.AddAsync(dto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!await _partidoService.CanModify()) return RedirectToAction("Index");

            var dto = await _partidoService.GetById(id);
            if (dto == null) return RedirectToAction("Index");

            ViewBag.EditMode = true;
            var vm = _mapper.Map<SavePartidoPoliticoViewModel>(dto);
            return View("Save", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SavePartidoPoliticoViewModel vm)
        {
            if (!await _partidoService.CanModify()) return RedirectToAction("Index");

            if (!ModelState.IsValid)
            {
                ViewBag.EditMode = true;
                return View("Save", vm);
            }

            if (await _partidoService.SiglasExistente(vm.Siglas, vm.Id))
            {
                ModelState.AddModelError("Siglas", "Ya existe un partido con estas siglas.");
                ViewBag.EditMode = true;
                return View("Save", vm);
            }

            var dto = _mapper.Map<PartidoPoliticoDto>(vm);

            if (vm.LogoFile != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(vm.LogoFile.FileName);
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img");
                Directory.CreateDirectory(folderPath);
                var fullPath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await vm.LogoFile.CopyToAsync(stream);
                }

                dto.LogoUrl = "/img/" + fileName;
            }
            else
            {
                dto.LogoUrl = vm.LogoUrl; //Mantener el logo actual si no se sube nuevo
            }

            await _partidoService.UpdateAsync(dto, dto.Id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ToggleEstado(int id)
        {
            if (!await _partidoService.CanModify()) return RedirectToAction("Index");

            var dto = await _partidoService.GetById(id);
            if (dto == null) return RedirectToAction("Index");

            var vm = _mapper.Map<ToggleEstadoPartidoViewModel>(dto);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleEstado(ToggleEstadoPartidoViewModel vm)
        {
            if (!await _partidoService.CanModify()) return RedirectToAction("Index");

            await _partidoService.ToggleEstadoAsync(vm.Id);
            return RedirectToAction("Index");
        }
    }
}
