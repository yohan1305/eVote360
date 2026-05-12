using AutoMapper;
using Evote365.Core.Application.Dtos.Dirigente.AsignacionCandidatos;
using Evote365.Core.Application.Interfaces.Administrador;
using Evote365.Core.Application.Interfaces.Dirigente;
using Evote366.Core.Domain.Common.Enums;
using Evote366.Core.Domain.Entities;
using Evote366.Core.Domain.Interfaces;


namespace Evote365.Core.Application.Services.Dirigente
{
    public class AsignacionCandidatoPuestoService
     : GenericService<Candidato, AsignacionCandidatoPuestoDto>, IAsignacionCandidatoPuestoService
    {
        private readonly ICandidatoRepository _candidatoRepository;
        private readonly IPuestoElectivoRepository _puestoRepository;
        private readonly IAlianzaPoliticaRepository _alianzaRepository;
        private readonly IUserSession _userSession;
        private readonly IEleccionRepository _eleccionRepository;
        private readonly IMapper _mapper;
        private readonly IAsignacionCandidatoPuestoRepository _asignacionRepository;

        public AsignacionCandidatoPuestoService(
            ICandidatoRepository candidatoRepository,
            IPuestoElectivoRepository puestoRepository,
            IAlianzaPoliticaRepository alianzaRepository,
            IUserSession userSession,
           IEleccionRepository eleccionRepository,
            IMapper mapper,
            IAsignacionCandidatoPuestoRepository asignacionRepository
        ) : base(candidatoRepository, mapper)
        {
            _candidatoRepository = candidatoRepository;
            _puestoRepository = puestoRepository;
            _alianzaRepository = alianzaRepository;
            _userSession = userSession;
            _eleccionRepository = eleccionRepository;
            _mapper = mapper;
            _asignacionRepository = asignacionRepository;
        }

        public async Task<bool> HayEleccionActivaAsync()
        {
            var elecciones = await _eleccionRepository.GetAllList();
            return elecciones.Any(e => e.Estado == EstadoEleccion.EnProceso);
        }

        public async Task<List<CandidatoDisponibleDto>> GetRelacionesExistentesAsync(int partidoId)
        {
            var asignados = await _candidatoRepository.GetAsignacionesConPuestoPorPartido(partidoId);
            return asignados.Select(c => _mapper.Map<CandidatoDisponibleDto>(c)).ToList();
        }

        public async Task<CandidatoDisponibleDto?> GetRelacionPorCandidatoAsync(int candidatoId)
        {
            var candidato = await _candidatoRepository.GetByIdWithInclude(candidatoId);
            if (candidato == null || !candidato.PuestoElectivoId.HasValue) return null;
            return _mapper.Map<CandidatoDisponibleDto>(candidato);
        }

        public async Task<ResultadoOperacionDto> ValidarAsignacionAsync(AsignacionCandidatoPuestoDto dto)
        {
            var partidoIdActual = _userSession.GetPartidoId();

            var candidato = await _candidatoRepository.GetByIdWithInclude(dto.CandidatoId);
            if (candidato == null)
            {
                return new ResultadoOperacionDto
                {
                    Exito = false,
                    Mensaje = "Candidato no encontrado."
                };
            }

            var puestoNuevo = await _puestoRepository.GetByIdAsync(dto.PuestoElectivoId);
            if (puestoNuevo == null || puestoNuevo.Estado != EstadoEntidad.Activo)
            {
                return new ResultadoOperacionDto
                {
                    Exito = false,
                    Mensaje = "Puesto no válido o inactivo."
                };
            }

            var esDelPartido = candidato.PartidoPoliticoId == partidoIdActual;
            var esAliado = await _alianzaRepository.ExisteAlianzaEntre(partidoIdActual, candidato.PartidoPoliticoId);

            if (!esDelPartido && !esAliado)
            {
                return new ResultadoOperacionDto
                {
                    Exito = false,
                    Mensaje = $"No puede asignar candidatos del partido {candidato.PartidoPoliticoId}. No hay alianza con su partido ({partidoIdActual})."
                };
            }

            // Obtener todas las asignaciones del candidato
            var asignaciones = await _asignacionRepository.GetAsignacionesPorCandidatoAsync(dto.CandidatoId);

            // Si es aliado, debe tener ya asignado ese mismo puesto en su partido original
            if (esAliado)
            {
                var tieneAsignacionValida = asignaciones.Any(a =>
                    a.PartidoPoliticoId == candidato.PartidoPoliticoId &&
                    a.PuestoElectivoId == dto.PuestoElectivoId);

                if (!tieneAsignacionValida)
                {
                    return new ResultadoOperacionDto
                    {
                        Exito = false,
                        Mensaje = "Este candidato aliado no tiene asignado ese puesto en su partido original."
                    };
                }
            }

            return new ResultadoOperacionDto { Exito = true };
        }

        public async Task<ResultadoOperacionDto> AsignarAsync(AsignacionCandidatoPuestoDto dto)
        {
            var validacion = await ValidarAsignacionAsync(dto);
            var partidoId = _userSession.GetPartidoId();

            var asignacionesExistentes = await _asignacionRepository.GetAsignacionesPorCandidatoAsync(dto.CandidatoId);

            bool yaTieneOtroPuestoEnEstePartido = asignacionesExistentes
                .Any(a => a.PartidoPoliticoId == partidoId && a.PuestoElectivoId != dto.PuestoElectivoId);

            if (yaTieneOtroPuestoEnEstePartido)
            {
                return new ResultadoOperacionDto
                {
                    Exito = false,
                    Mensaje = "Este candidato ya tiene un puesto asignado en este partido."
                };
            }

            if (!validacion.Exito) return validacion;

            var yaExiste = await _asignacionRepository.ExisteAsignacionAsync(
                dto.CandidatoId, _userSession.GetPartidoId(), dto.PuestoElectivoId);
            if (yaExiste)
            {
                return new ResultadoOperacionDto
                {
                    Exito = false,
                    Mensaje = "Este candidato ya está asignado a este puesto en este partido."
                };
            }

            var nuevaAsignacion = new AsignacionCandidatoPuesto
            {
                CandidatoId = dto.CandidatoId,
                PartidoPoliticoId = _userSession.GetPartidoId(),
                PuestoElectivoId = dto.PuestoElectivoId,
                FechaAsignacion = DateTime.Now
            };

            await _asignacionRepository.AddAsync(nuevaAsignacion);

            return new ResultadoOperacionDto
            {
                Exito = true,
                Mensaje = "Asignación registrada correctamente."
            };
        }

        public async Task<ResultadoOperacionDto> DesvincularAsync(int candidatoId)
        {
            if (await HayEleccionActivaAsync())
                return new ResultadoOperacionDto { Exito = false, Mensaje = "Operación denegada. La Elección ya está activa." };

            var partidoId = _userSession.GetPartidoId();

            var asignaciones = await _asignacionRepository.GetAsignacionesPorCandidatoAsync(candidatoId);
            var asignacion = asignaciones.FirstOrDefault(a => a.PartidoPoliticoId == partidoId);

            if (asignacion == null)
                return new ResultadoOperacionDto { Exito = false, Mensaje = "No hay asignación registrada para desvincular." };

            await _asignacionRepository.DeleteAsync(asignacion.Id);

            return new ResultadoOperacionDto { Exito = true, Mensaje = "Desvinculación completada." };
        }

        public async Task<List<CandidatoDisponibleDto>> GetCandidatosDisponiblesAsync(int puestoId)
        {
            var partidoId = _userSession.GetPartidoId();
            
            var aliadosIds = await _alianzaRepository.GetAliadosVigentesIdsAsync(partidoId);
            var candidatos = await _candidatoRepository.GetCandidatosDisponiblesParaAsignacion(partidoId, aliadosIds, puestoId);
            var asignaciones = await _asignacionRepository.GetAsignacionesPorCandidatosAsync(candidatos.Select(c => c.Id).ToList());

            var resultado = new List<CandidatoDisponibleDto>();

            foreach (var candidato in candidatos)
            {
                var dto = _mapper.Map<CandidatoDisponibleDto>(candidato);

                // Buscar asignación que coincida con el puesto actual (para mostrarlo en el select)
                var asignacionOriginal = asignaciones
             .FirstOrDefault(a => a.CandidatoId == candidato.Id && a.PartidoPoliticoId == candidato.PartidoPoliticoId);

                dto.YaTienePuesto = asignaciones.Any(a => a.CandidatoId == candidato.Id && a.PartidoPoliticoId == partidoId);
                dto.NombrePuestoActual = asignacionOriginal?.PuestoElectivo?.Nombre ?? "Sin puesto";
                dto.EsAliado = aliadosIds.Contains(candidato.PartidoPoliticoId);

                resultado.Add(dto);
            }

            return resultado;
        }

        public async Task<List<PuestoDisponibleDto>> GetPuestosDisponiblesAsync()
        {
            var partidoId = _userSession.GetPartidoId();
            var puestos = await _puestoRepository.GetDisponiblesParaPartido(partidoId);
            return puestos.Select(p => _mapper.Map<PuestoDisponibleDto>(p)).ToList();
        }

        public async Task<List<AsignacionCandidatoPuestoDto>> GetAsignacionesPorPartidoAsync(int partidoId)
        {
            var asignaciones = await _asignacionRepository.GetAsignacionesPorPartidoAsync(partidoId);
            return asignaciones.Select(a => _mapper.Map<AsignacionCandidatoPuestoDto>(a)).ToList();
        }

        public async Task<List<AsignacionCandidatoPuestoDto>> GetAsignacionesPorPuestoAsync(int puestoId)
        {
            var asignaciones = await _asignacionRepository.GetAsignacionesPorPuestoAsync(puestoId);
            return asignaciones.Select(a => _mapper.Map<AsignacionCandidatoPuestoDto>(a)).ToList();
        }




    }
}
