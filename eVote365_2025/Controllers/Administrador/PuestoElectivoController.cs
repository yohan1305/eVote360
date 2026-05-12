using AutoMapper;
using Evote365.Core.Application.Dtos.Administrador.PuestoElectivoDto;
using Evote365.Core.Application.Interfaces.Administrador;
using Evote365.Core.Application.ViewModels.Administrador.PuestoElectivo;
using Microsoft.AspNetCore.Mvc;

namespace eVote365_2025.Controllers.Administrador
{
    [ValidarRol("Administrador")]
    public class PuestoElectivoController : Controller
    {
        private readonly IPuestoElectivoService _puestoService;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;

        public PuestoElectivoController(IPuestoElectivoService puestoService, IUserSession userSession, IMapper mapper)
        {
            _puestoService = puestoService;
            _userSession = userSession;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var dtos = await _puestoService.GetAll();
            var vms = _mapper.Map<List<PuestoElectivoViewModel>>(dtos);
            return View("Index", vms);
        }

        public async Task<IActionResult> Create()
        {
            //if (!_userSession.HasUser()) return RedirectToRoute(new { controller = "Login", action = "Index" });
            //if (!_userSession.IsAdmin()) return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

            if (!await _puestoService.CanModify()) return RedirectToAction("Index");

            return View("Save", new SavePuestoElectivoViewModel
            {
                Nombre = "",
                Descripcion = ""
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(SavePuestoElectivoViewModel vm)
        {
            //if (!_userSession.HasUser()) return RedirectToRoute(new { controller = "Login", action = "Index" });
            //if (!_userSession.IsAdmin()) return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

            if (!ModelState.IsValid) return View("Save", vm);

            var dto = _mapper.Map<PuestoElectivoDto>(vm);
            await _puestoService.AddAsync(dto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            //if (!_userSession.HasUser()) return RedirectToRoute(new { controller = "Login", action = "Index" });
            //if (!_userSession.IsAdmin()) return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

            if (!await _puestoService.CanModify()) return RedirectToAction("Index");

            var dto = await _puestoService.GetById(id);
            if (dto == null) return RedirectToAction("Index");

            ViewBag.EditMode = true;
            var vm = _mapper.Map<SavePuestoElectivoViewModel>(dto);
            return View("Save", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SavePuestoElectivoViewModel vm)
        {
            //if (!_userSession.HasUser()) return RedirectToRoute(new { controller = "Login", action = "Index" });
            //if (!_userSession.IsAdmin()) return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

            if (!ModelState.IsValid)
            {
                ViewBag.EditMode = true;
                return View(vm);
            }

            var dto = _mapper.Map<PuestoElectivoDto>(vm);
            await _puestoService.UpdateAsync(dto, dto.Id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ToggleEstado(int id)
        {
            //if (!_userSession.HasUser()) return RedirectToRoute(new { controller = "Login", action = "Index" });
            //if (!_userSession.IsAdmin()) return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

            await _puestoService.ToggleEstadoAsync(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            //if (!_userSession.HasUser()) return RedirectToRoute(new { controller = "Login", action = "Index" });
            //if (!_userSession.IsAdmin()) return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

            var dto = await _puestoService.GetById(id);
            if (dto == null) return RedirectToAction("Index");

            var vm = _mapper.Map<DeletePuestoElectivoViewModel>(dto);
            return View("Delete",vm);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(DeletePuestoElectivoViewModel vm)
        {
            //if (!_userSession.HasUser()) return RedirectToRoute(new { controller = "Login", action = "Index" });
            //if (!_userSession.IsAdmin()) return RedirectToRoute(new { controller = "Login", action = "AccessDenied" });

            await _puestoService.DeleteAsync(vm.Id);
            return RedirectToAction("Index");
        }

    }
}
