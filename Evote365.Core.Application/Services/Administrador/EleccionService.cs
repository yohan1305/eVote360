using AutoMapper;
using Evote365.Core.Application.Dtos.Administrador.Eleccion;
using Evote365.Core.Application.Dtos.Dirigente.AsignacionCandidatos;
using Evote365.Core.Application.Interfaces.Administrador;
using Evote366.Core.Domain.Common.Enums;
using Evote366.Core.Domain.Entities;
using Evote366.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Evote365.Core.Application.Services.Administrador
{
    public class EleccionService : GenericService<Eleccion, EleccionDto>, IEleccionService
    {
        private readonly IEleccionRepository _eleccionRepository;
        private readonly IPuestoElectivoRepository _puestoRepository;
        private readonly IPartidoPoliticoRepository _partidoRepository;
        private readonly ICandidatoRepository _candidatoRepository;
        private readonly IAsignacionCandidatoPuestoRepository _asignacionRepository;
        private readonly IMapper _mapper;

        public EleccionService(
            IEleccionRepository eleccionRepository,
            IPuestoElectivoRepository puestoRepository,
            IPartidoPoliticoRepository partidoRepository,
            ICandidatoRepository candidatoRepository,
            IAsignacionCandidatoPuestoRepository asignacionCandidatoPuestoRepository,
            IMapper mapper,
            IAlianzaPoliticaRepository alianzaRepository) : base(eleccionRepository, mapper)
        {
            _eleccionRepository = eleccionRepository;
            _puestoRepository = puestoRepository;
            _partidoRepository = partidoRepository;
            _candidatoRepository = candidatoRepository;
            _asignacionRepository = asignacionCandidatoPuestoRepository;
            _mapper = mapper;
        }

        public override async Task<List<EleccionDto>> GetAll()
        {
            var elecciones = await _eleccionRepository
                .GetAllQueryWithInclude(["PuestosEnEleccion", "PuestosEnEleccion.PuestoElectivo", "PuestosEnEleccion.PuestoElectivo.Asignaciones", "VotosEmitidos"])
                .OrderByDescending(e => e.Estado == EstadoEleccion.EnProceso)
                .ThenByDescending(e => e.FechaRealizacion)
                .ToListAsync();

            return elecciones.Select(eleccion =>
            {
                var dto = _mapper.Map<EleccionDto>(eleccion);
                var asignaciones = eleccion.PuestosEnEleccion.SelectMany(pe => pe.PuestoElectivo.Asignaciones).ToList();

                dto.CantidadPuestos = eleccion.PuestosEnEleccion.Select(pe => pe.PuestoElectivoId).Distinct().Count();
                dto.CantidadPartidos = asignaciones.Select(a => a.PartidoPoliticoId).Distinct().Count();
                dto.CantidadCandidatos = asignaciones.Select(a => a.CandidatoId).Distinct().Count();
                dto.TotalVotosEmitidos = eleccion.VotosEmitidos.Select(v => v.CiudadanoId).Distinct().Count();

                return dto;
            }).ToList();
        }

        public async Task<bool> HayEleccionActiva()
        {
            return await _eleccionRepository.GetEleccionActiva() is not null;
        }

        public async Task<EleccionDto?> GetEleccionActiva()
        {
            var activa = await _eleccionRepository.GetEleccionActiva();
            return activa == null ? null : _mapper.Map<EleccionDto>(activa);
        }

        public async Task<(bool Exito, List<string> Mensajes)> CrearEleccionAsync(SaveEleccionDto dto)
        {
            var mensajes = new List<string>();

            if (await HayEleccionActiva())
            {
                mensajes.Add("Ya existe una elección activa.");
                return (false, mensajes);
            }

            var puestosActivos = await _puestoRepository.GetPuestosActivosAsync();
            if (!puestosActivos.Any())
            {
                mensajes.Add("No hay puestos electivos activos.");
                return (false, mensajes);
            }

            var partidosActivos = await _partidoRepository.GetActivos();
            if (partidosActivos.Count < 2)
            {
                mensajes.Add("No hay suficientes partidos políticos para realizar una elección.");
                return (false, mensajes);
            }

            foreach (var partido in partidosActivos)
            {
                // 1. Traemos las asignaciones del partido (Aquí Omar aparece vinculado al PLD)
                var asignaciones = await _asignacionRepository.GetAsignacionesPorPartidoAsync(partido.Id);

                // 2. Obtenemos los IDs de los puestos que este partido ha cubierto
                var idsPuestosCubiertos = asignaciones
                    .Select(a => a.PuestoElectivoId)
                    .Distinct()
                    .ToList();

                // 3. Comparamos contra los puestos que el Admin activó
                var puestosFaltantes = puestosActivos
                    .Where(p => !idsPuestosCubiertos.Contains(p.Id))
                    .Select(p => p.Nombre.Trim())
                    .ToList();

                if (puestosFaltantes.Any())
                {
                    mensajes.Add($"El partido {partido.Nombre} [{partido.Siglas}] no tiene candidatos para: {string.Join(", ", puestosFaltantes)}.");
                }
            }

            if (mensajes.Any())
            {
                return (false, mensajes);
            }

            var nuevaEleccion = _mapper.Map<Eleccion>(dto);
            nuevaEleccion.Estado = EstadoEleccion.EnProceso;

            var eleccionCreada = await _eleccionRepository.AddAsync(nuevaEleccion);
            if (eleccionCreada is null)
            {
                return (false, ["No se pudo crear la elección."]);
            }

            await _eleccionRepository.AsignarPuestosAEleccionAsync(
                eleccionCreada.Id,
                puestosActivos.Select(p => p.Id).ToList());

            return (true, []);
        }

        public async Task<bool> FinalizarEleccionAsync(int eleccionId)
        {
            var eleccion = await _eleccionRepository.GetById(eleccionId);
            if (eleccion == null || eleccion.Estado != EstadoEleccion.EnProceso)
            {
                return false;
            }

            eleccion.Estado = EstadoEleccion.Finalizada;
            await _eleccionRepository.UpdateAsync(eleccionId, eleccion);
            return true;
        }

        public async Task<ResultadoEleccionDto?> GetResultadosPorEleccion(int eleccionId)
        {
            var eleccion = await _eleccionRepository
                .GetAllQueryWithInclude([
                    "PuestosEnEleccion",
            "PuestosEnEleccion.PuestoElectivo",
            "PuestosEnEleccion.PuestoElectivo.Asignaciones",
            "PuestosEnEleccion.PuestoElectivo.Asignaciones.Candidato",
            "PuestosEnEleccion.PuestoElectivo.Asignaciones.PartidoPolitico",
            "VotosEmitidos"
                ])
                .FirstOrDefaultAsync(e => e.Id == eleccionId && e.Estado == EstadoEleccion.Finalizada);

            if (eleccion == null)
            {
                return null;
            }

            var resultado = _mapper.Map<ResultadoEleccionDto>(eleccion);
            resultado.ResultadosPorPuesto = new List<ResultadoPuestoDto>(); // Asegurar inicialización

            var puestos = eleccion.PuestosEnEleccion.Select(pe => pe.PuestoElectivo).ToList();

            foreach (var puesto in puestos)
            {
                // 1. Agrupamos las asignaciones incluyendo el ID del partido para diferenciar alianzas
                var candidatosInfo = puesto.Asignaciones
                    .Where(a => a.Candidato.Estado == EstadoEntidad.Activo)
                    .GroupBy(a => new
                    {
                        a.CandidatoId,
                        a.Candidato.Nombre,
                        a.Candidato.Apellido,
                        a.PartidoPoliticoId, // <-- Clave para diferenciar el recuadro de votación
                        PartidoSiglas = a.PartidoPolitico.Siglas
                    })
                    .Select(g => new
                    {
                        g.Key.CandidatoId,
                        g.Key.PartidoPoliticoId,
                        NombreCompleto = $"{g.Key.Nombre} {g.Key.Apellido}",
                        g.Key.PartidoSiglas
                    })
                    .ToList();

                // 2. Filtramos los votos que pertenecen a este puesto específico
                var votosPorPuesto = eleccion.VotosEmitidos
                    .Where(v => v.PuestoElectivoId == puesto.Id)
                    .ToList();

                var totalVotosPuesto = votosPorPuesto.Count;

                // 3. Calculamos resultados por cada "recuadro" (Candidato en un Partido específico)
                var candidatosDto = candidatosInfo
                    .Select(c => {
                        // Contamos solo los votos donde coincidan Candidato Y Partido
                        int votosRecibidos = votosPorPuesto.Count(v =>
                            v.CandidatoId == c.CandidatoId &&
                            v.PartidoPoliticoId == c.PartidoPoliticoId);

                        return new ResultadoCandidatoDto
                        {
                            NombreCandidato = c.NombreCompleto,
                            PartidoSiglas = c.PartidoSiglas,
                            VotosRecibidos = votosRecibidos,
                            Porcentaje = totalVotosPuesto == 0 ? 0 :
                                Math.Round((decimal)votosRecibidos / totalVotosPuesto * 100, 2)
                        };
                    })
                    .OrderByDescending(c => c.VotosRecibidos) // El ganador arriba
                    .ToList();

                resultado.ResultadosPorPuesto.Add(new ResultadoPuestoDto
                {
                    NombrePuesto = puesto.Nombre,
                    Candidatos = candidatosDto
                });
            }

            return resultado;
        }

        public async Task<Eleccion?> GetEleccionForActiva()
        {
            return await _eleccionRepository.GetEleccionActivaForElector();
        }

        public async Task<EleccionDto?> GetEleccionActivaAsync()
        {
            var eleccion = await _eleccionRepository.GetEleccionActiva();
            return eleccion is null ? null : _mapper.Map<EleccionDto>(eleccion);
        }

        public async Task<EleccionDto?> GetEleccionActivaForElectorAsync()
        {
            var eleccion = await _eleccionRepository.GetEleccionActivaForElector();
            return eleccion is null ? null : _mapper.Map<EleccionDto>(eleccion);
        }

        public async Task<List<PuestoAsignableDto>> ObtenerPuestosAsignablesAsync(int eleccionId)
        {
            var puestos = await _puestoRepository.GetAllList();
            var asignados = await _eleccionRepository.GetPuestosAsignadosAsync(eleccionId);

            return puestos.Select(p =>
            {
                var dto = _mapper.Map<PuestoAsignableDto>(p);
                dto.EstaAsignado = asignados.Contains(p.Id);
                return dto;
            }).ToList();
        }

        public async Task AsignarPuestosAEleccionAsync(AsignacionPuestosDto dto)
        {
            await _eleccionRepository.AsignarPuestosAEleccionAsync(dto.EleccionId, dto.PuestosSeleccionados);
        }
    }
}
