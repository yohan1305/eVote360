using AutoMapper;
using Evote365.Core.Application.Dtos.Elector;
using Evote365.Core.Application.Interfaces;
using Evote365.Core.Application.Interfaces.Administrador;
using Evote365.Core.Application.Interfaces.Elector;
using Evote365.Core.Application.ViewModels.Elector;
using Evote366.Core.Domain.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace eVote365_2025.Controllers.Elector
{
    public class ElectorController : Controller
    {
        private readonly IElectorService _electorService;
        private readonly IEleccionService _eleccionService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public ElectorController(
            IElectorService electorService,
            IEleccionService eleccionService,
            IEmailService emailService,
            IMapper mapper)
        {
            _electorService = electorService;
            _eleccionService = eleccionService;
            _emailService = emailService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult VerificarCedula()
        {
            return View(new VerificarCedulaViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> VerificarCedula(VerificarCedulaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var eleccionActiva = await _eleccionService.GetEleccionActivaForElectorAsync();
            if (eleccionActiva is null)
            {
                model.MensajeError = "No hay una elección activa en este momento.";
                return View(model);
            }

            var ciudadano = await _electorService.ObtenerPorDocumentoAsync(model.DocumentoIdentidad);
            if (ciudadano is null)
            {
                model.MensajeError = "No se encontró un ciudadano con esa cédula.";
                return View(model);
            }

            if (ciudadano.Estado != EstadoEntidad.Activo)
            {
                model.MensajeError = "Este ciudadano no está habilitado para votar.";
                return View(model);
            }

            if (await _electorService.YaHaVotadoAsync(model.DocumentoIdentidad, eleccionActiva.Id))
            {
                model.MensajeError = "Ya ha ejercido su derecho al voto en la elección activa.";
                return View(model);
            }

            return RedirectToAction("ValidarIdentidad", new { cedula = model.DocumentoIdentidad });
        }

        [HttpGet]
        public IActionResult ValidarIdentidad(string cedula)
        {
            if (string.IsNullOrWhiteSpace(cedula))
            {
                return RedirectToAction("VerificarCedula");
            }

            return View(new ValidarIdentidadViewModel
            {
                Cedula = cedula
            });
        }

        [HttpGet]
        public async Task<IActionResult> ListarPuestos(string cedula)
        {
            if (string.IsNullOrWhiteSpace(cedula))
            {
                return RedirectToAction("VerificarCedula");
            }

            var eleccion = await _eleccionService.GetEleccionForActiva();
            if (eleccion is null)
            {
                return RedirectToAction("VerificarCedula");
            }

            var puestos = eleccion.PuestosEnEleccion
                .Where(p => p.PuestoElectivo.Estado == EstadoEntidad.Activo)
                .Select(p => new PuestoResumenViewModel
                {
                    PuestoId = p.PuestoElectivo.Id,
                    NombrePuesto = p.PuestoElectivo.Nombre,
                    CantidadPartidos = p.PuestoElectivo.Asignaciones.Select(a => a.PartidoPoliticoId).Distinct().Count(),
                    CantidadCandidatosReales = p.PuestoElectivo.Asignaciones.Select(a => a.CandidatoId).Distinct().Count()
                })
                .ToList();

            var puestosVotados = await _electorService.GetPuestosVotadosAsync(cedula, eleccion.Id);
            foreach (var puesto in puestos)
            {
                puesto.YaVotado = puestosVotados.Contains(puesto.PuestoId);
            }

            ViewBag.Cedula = cedula;
            ViewBag.MensajePendientes = TempData["MensajePendientes"]?.ToString();
            return View(puestos);
        }

        [HttpGet]
        public async Task<IActionResult> SeleccionarCandidato(int puestoId, string cedula)
        {
            if (string.IsNullOrWhiteSpace(cedula))
            {
                return RedirectToAction("VerificarCedula");
            }

            var eleccion = await _eleccionService.GetEleccionForActiva();
            if (eleccion is null)
            {
                return RedirectToAction("VerificarCedula");
            }

            var puesto = eleccion.PuestosEnEleccion.FirstOrDefault(p => p.PuestoElectivo.Id == puestoId);
            if (puesto is null)
            {
                return RedirectToAction("ListarPuestos", new { cedula });
            }

            return View(BuildSeleccionViewModel(puestoId, cedula, puesto.PuestoElectivo.Nombre, puesto.PuestoElectivo.Asignaciones));
        }

        [HttpPost]
        public async Task<IActionResult> SeleccionarCandidato(SeleccionarCandidatoViewModel model)
        {
            // 1. Validación de seguridad: Si alguno es nulo, devolvemos a la vista con error
            if (model.CandidatoIdSeleccionado == null || model.PartidoIdSeleccionado == null)
            {
                model.MensajeError = "Debe hacer clic en la tarjeta de un candidato para votar.";
                return await RebuildSeleccionCandidatoAsync(model);
            }

            var eleccion = await _eleccionService.GetEleccionForActiva();
            if (eleccion is null) return RedirectToAction("VerificarCedula");

            // 2. Ahora es seguro usar .Value porque ya validamos arriba
            var registrado = await _electorService.RegistrarVotoAsync(
                model.CedulaElector,
                eleccion.Id,
                model.PuestoId,
                model.CandidatoIdSeleccionado.Value,
                model.PartidoIdSeleccionado.Value);

            if (!registrado)
            {
                model.MensajeError = "No se pudo registrar el voto. Verifique si ya votó en este puesto.";
                return await RebuildSeleccionCandidatoAsync(model);
            }

            return RedirectToAction("ListarPuestos", new { cedula = model.CedulaElector });
        }

        [HttpGet]
        public async Task<IActionResult> FinalizarVotacion(string cedula)
        {
            if (string.IsNullOrWhiteSpace(cedula))
            {
                return RedirectToAction("VerificarCedula");
            }

            var eleccion = await _eleccionService.GetEleccionForActiva();
            if (eleccion is null)
            {
                return RedirectToAction("VerificarCedula");
            }

            var puestosTotales = eleccion.PuestosEnEleccion.Select(p => p.PuestoElectivo.Id).ToList();
            var puestosVotados = await _electorService.GetPuestosVotadosAsync(cedula, eleccion.Id);
            var faltantes = puestosTotales
                .Except(puestosVotados)
                .Select(id => eleccion.PuestosEnEleccion.First(p => p.PuestoElectivo.Id == id).PuestoElectivo.Nombre)
                .ToList();

            var model = new FinalizarVotacionViewModel
            {
                CedulaElector = cedula,
                PuestosFaltantes = faltantes,
                ResumenVotos = await _electorService.ObtenerResumenVotosAsync(cedula, eleccion.Id)
            };

            if (!model.VotacionCompleta)
            {
                TempData["MensajePendientes"] = $"Aún faltan votos por registrar para: {string.Join(", ", faltantes)}.";
                return RedirectToAction("ListarPuestos", new { cedula });
            }

            await _electorService.FinalizarVotacionAsync(cedula, eleccion.Id);

            var ciudadano = await _electorService.ObtenerPorDocumentoAsync(cedula);
            if (ciudadano is not null && !string.IsNullOrWhiteSpace(ciudadano.Email))
            {
                await _emailService.SendAsync(
                    ciudadano.Email,
                    $"Resumen de votación - {eleccion.Nombre}",
                    ConstruirResumenCorreo($"{ciudadano.Nombre} {ciudadano.Apellido}", eleccion.Nombre, model.ResumenVotos));
            }

            model.Mensaje = "Su proceso de votación ha sido completado correctamente.";
            return View(model);
        }

        private SeleccionarCandidatoViewModel BuildSeleccionViewModel(
            int puestoId,
            string cedula,
            string nombrePuesto,
            IEnumerable<Evote366.Core.Domain.Entities.AsignacionCandidatoPuesto> asignaciones)
        {
            return new SeleccionarCandidatoViewModel
            {
                PuestoId = puestoId,
                NombrePuesto = nombrePuesto,
                CedulaElector = cedula,
                Candidatos = _mapper.Map<List<CandidatoParaVotarViewModel>>(
                    asignaciones.Where(a => a.Candidato.Estado == EstadoEntidad.Activo).ToList())
            };
        }

        private async Task<IActionResult> RebuildSeleccionCandidatoAsync(SeleccionarCandidatoViewModel model)
        {
            var eleccion = await _eleccionService.GetEleccionForActiva();
            if (eleccion is null)
            {
                return RedirectToAction("VerificarCedula");
            }

            var puesto = eleccion.PuestosEnEleccion.FirstOrDefault(p => p.PuestoElectivo.Id == model.PuestoId);
            if (puesto is null)
            {
                return RedirectToAction("ListarPuestos", new { cedula = model.CedulaElector });
            }

            var rebuilt = BuildSeleccionViewModel(model.PuestoId, model.CedulaElector, puesto.PuestoElectivo.Nombre, puesto.PuestoElectivo.Asignaciones);
            rebuilt.CandidatoIdSeleccionado = model.CandidatoIdSeleccionado;
            rebuilt.MensajeError = model.MensajeError;
            return View(rebuilt);
        }

        private static string ConstruirResumenCorreo(string nombreElector, string nombreEleccion, IEnumerable<VotoResumenItemDto> votos)
        {
            var filas = string.Join(string.Empty, votos.Select(v =>
                $"<tr><td>{v.Puesto}</td><td>{v.Candidato}</td><td>{v.Partido}</td></tr>"));

            var builder = new StringBuilder();
            builder.Append($"<p>Hola {nombreElector},</p>");
            builder.Append($"<p>Este es el resumen de su participación en <strong>{nombreEleccion}</strong>.</p>");
            builder.Append("<table border='1' cellpadding='8' cellspacing='0' style='border-collapse:collapse;'>");
            builder.Append("<thead><tr><th>Puesto</th><th>Candidato</th><th>Partido</th></tr></thead>");
            builder.Append($"<tbody>{filas}</tbody>");
            builder.Append("</table>");
            builder.Append("<p>Gracias por ejercer su derecho al voto.</p>");
            return builder.ToString();
        }
    }
}
