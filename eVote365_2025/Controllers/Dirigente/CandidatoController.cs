using AutoMapper;
using Evote365.Core.Application.Interfaces.Administrador;
using Evote365.Core.Application.Interfaces.Dirigente;
using Evote365.Core.Application.ViewModels.Dirigente.Candidato;
using Microsoft.AspNetCore.Mvc;

namespace eVote365_2025.Controllers.Dirigente
{

    [ValidarRol("DirigentePolitico")]
    public class CandidatoController : Controller
    {
        private readonly ICandidatoService _candidatoService;
        private readonly IMapper _mapper;

        public CandidatoController(
            ICandidatoService candidatoService,
            IMapper mapper)
        {
            _candidatoService = candidatoService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var dto = await _candidatoService.GetByPartidoDirigente();
            var puedeModificar = await _candidatoService.CanModify();
            var vm = _mapper.Map<CandidatoIndexViewModel>(dto);
            vm.PuedeModificar = puedeModificar;

            return View("Index", vm);
        }

        public IActionResult Crear()
        {
            var vm = new SaveCandidatoViewModel();
            return View("Save", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Crear(SaveCandidatoViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("Save", vm);

            await _candidatoService.CrearAsync(vm.Candidato);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Editar(int id)
        {
            var dto = await _candidatoService.GetSaveDtoById(id);
            if (dto == null) return RedirectToAction("Index");

            var vm = new SaveCandidatoViewModel
            {
                Candidato = dto
            };

            return View("Save", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(SaveCandidatoViewModel vm)
        {
            if (!ModelState.IsValid)
                return View("Save", vm);

            await _candidatoService.EditarAsync(vm.Candidato);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> ConfirmarEstado(int id, bool activar)
        {
            var dto = await _candidatoService.GetConfirmacionCambioEstado(id, activar);
            if (dto == null) return RedirectToAction("Index");

            var vm = _mapper.Map<ConfirmarCambioEstadoCandidatoViewModel>(dto);
            return View("ConfirmarEstado", vm);
        }

        [HttpPost]
        public async Task<IActionResult> CambiarEstado(ConfirmarCambioEstadoCandidatoViewModel vm)
        {
            await _candidatoService.CambiarEstadoAsync(vm.Candidato.Id, vm.Candidato.Activar);
            return RedirectToAction("Index");
        }
    }

}