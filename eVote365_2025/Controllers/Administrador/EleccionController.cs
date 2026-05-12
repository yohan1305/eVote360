using AutoMapper;
using Evote365.Core.Application.Dtos.Administrador.Eleccion;
using Evote365.Core.Application.Dtos.Dirigente.AsignacionCandidatos;
using Evote365.Core.Application.Interfaces.Administrador;
using Evote365.Core.Application.ViewModels.Administrador.Eleccion;
using Evote366.Core.Domain.Common.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eVote365_2025.Controllers.Administrador
{
    [ValidarRol("Administrador")]
    public class EleccionController : Controller
    {
        private readonly IEleccionService _eleccionService;
        private readonly IMapper _mapper;

        public EleccionController(IEleccionService eleccionService, IMapper mapper)
        {
            _eleccionService = eleccionService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var elecciones = await _eleccionService.GetAll();
            var activa = await _eleccionService.GetEleccionActiva();

            var viewModel = new EleccionListadoViewModel
            {
                Elecciones = elecciones,
                HayEleccionActiva = activa != null,
                EleccionActivaId = activa?.Id
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            var vm = new CrearEleccionViewModel
            {
                FechaRealizacion = DateTime.Today
            };

            return View(vm);
        }


        [HttpPost]
        public async Task<IActionResult> Crear(CrearEleccionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var dto = new SaveEleccionDto
            {
                Nombre = model.Nombre,
                FechaRealizacion = model.FechaRealizacion
            };

            var resultado = await _eleccionService.CrearEleccionAsync(dto);

            if (!resultado.Exito)
            {
                model.MensajesError = resultado.Mensajes;
                return View(model);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmarFinalizacion(int id)
        {
            var eleccion = await _eleccionService.GetById(id);
            if (eleccion == null || eleccion.Estado != EstadoEleccion.EnProceso)
            {
                return RedirectToAction("Index");
            }

            var viewModel = _mapper.Map<ConfirmarFinalizacionViewModel>(eleccion);
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Finalizar(ConfirmarFinalizacionViewModel model)
        {
            var resultado = await _eleccionService.FinalizarEleccionAsync(model.EleccionId);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Resultados(int id)
        {
            var resultado = await _eleccionService.GetResultadosPorEleccion(id);
            if (resultado == null)
            {
                return RedirectToAction("Index");
            }

            var viewModel = _mapper.Map<ResultadoEleccionViewModel>(resultado);
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> AsignarPuestos(int id)
        {
            var puestosDto = await _eleccionService.ObtenerPuestosAsignablesAsync(id);
            var viewModel = new AsignacionPuestosViewModel
            {
                EleccionId = id,
                PuestosDisponibles = _mapper.Map<List<PuestoAsignableViewModel>>(puestosDto)
            };

            return View(viewModel); // Vista: Views/Eleccion/AsignarPuestos.cshtml
        }

        [HttpPost]
        public async Task<IActionResult> AsignarPuestos(AsignacionPuestosViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var dto = _mapper.Map<AsignacionPuestosDto>(model);
            await _eleccionService.AsignarPuestosAEleccionAsync(dto);

            return RedirectToAction("Index");
        }




    }
}
