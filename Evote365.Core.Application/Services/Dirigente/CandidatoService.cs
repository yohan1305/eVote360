using AutoMapper;
using Evote365.Core.Application.Dtos.Dirigente.Candidato;
using Evote365.Core.Application.Interfaces;
using Evote365.Core.Application.Interfaces.Administrador;
using Evote365.Core.Application.Interfaces.Dirigente;
using Evote366.Core.Domain.Common.Enums;
using Evote366.Core.Domain.Entities;
using Evote366.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace Evote365.Core.Application.Services.Dirigente
{
    public class CandidatoService : GenericService<Candidato, CandidatoDto>, ICandidatoService
    {
        private readonly IAsignacionCandidatoPuestoRepository  _asignacionRepository;
        private readonly ICandidatoRepository _candidatoRepository;
        private readonly IEleccionRepository _eleccionRepository;
        private readonly IUserSession _userSession;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;
        private readonly IPartidoPoliticoRepository _partidoRepository;
        public CandidatoService(
            ICandidatoRepository candidatoRepository,
            IEleccionRepository eleccionRepository,
            IPartidoPoliticoRepository partidoRepository,
            IUserSession userSession,
            IMapper mapper,
            IUploadService uploadService,
            IAsignacionCandidatoPuestoRepository asignacionRepository
        ) : base(candidatoRepository, mapper)
        {
            _candidatoRepository = candidatoRepository;
            _eleccionRepository = eleccionRepository;
            _partidoRepository = partidoRepository;
            _userSession = userSession;
            _mapper = mapper;
            _uploadService = uploadService;
            _asignacionRepository = asignacionRepository;
        }

        public async Task<List<CandidatoDto>> GetByPartidoDirigente()
        {
            int partidoId = _userSession.GetPartidoId();
            var candidatos = await _candidatoRepository.GetByPartidoIdWithInclude(partidoId);
            var dtos = _mapper.Map<List<CandidatoDto>>(candidatos);

            foreach (var dto in dtos)
            {
                var asignaciones = await _asignacionRepository.GetAsignacionesPorCandidatoAsync(dto.Id);
                dto.PuestosAsignados = asignaciones
                    .Select(a => $"{a.PuestoElectivo.Nombre} ({a.PartidoPolitico.Siglas})")
                    .ToList();
            }

            return dtos;
        }

        public async Task<bool> CrearAsync(SaveCandidatoDto dto)
        {
            if (!await CanModify()) return false;

            int partidoId =  _userSession.GetPartidoId();
            var entity = _mapper.Map<Candidato>(dto);
            entity.Estado = EstadoEntidad.Activo;
            entity.PartidoPoliticoId = partidoId;

            if (dto.Foto != null)
            {
                entity.FotoUrl = await _uploadService.SaveFileAsync(dto.Foto);
            }

            var saved = await _candidatoRepository.AddAsync(entity);
            return saved != null;
        }

        public async Task<SaveCandidatoDto?> GetSaveDtoById(int id)
        {
            var entity = await _candidatoRepository.GetById(id);
            if (entity == null) return null;

            var dto = _mapper.Map<SaveCandidatoDto>(entity);
            return dto;
        }

        public async Task<bool> EditarAsync(SaveCandidatoDto dto)
        {
            if (!await CanModify()) return false;


            if (!dto.Id.HasValue) return false;
            var actual = await _candidatoRepository.GetById(dto.Id.Value);
            if (actual == null) return false;

            var entity = _mapper.Map<Candidato>(dto);
            entity.PuestoElectivoId = actual.PuestoElectivoId;
            entity.Id = actual.Id;
            entity.Estado = actual.Estado;
            entity.PartidoPoliticoId = actual.PartidoPoliticoId;

            entity.FotoUrl = actual.FotoUrl;
            if (dto.Foto != null)
            {
                entity.FotoUrl = await _uploadService.SaveFileAsync(dto.Foto);
            }

            var updated = await _candidatoRepository.UpdateAsync(entity.Id, entity);
            return updated != null;
        }

        public async Task<ConfirmarCambioEstadoCandidatoDto?> GetConfirmacionCambioEstado(int id, bool activar)
        {
            var entity = await _candidatoRepository.GetById(id);
            if (entity == null) return null;

            var dto = _mapper.Map<ConfirmarCambioEstadoCandidatoDto>(entity);
            dto.Activar = activar;
            return dto;
        }

        public async Task<bool> CambiarEstadoAsync(int id, bool activar)
        {
            if (!await CanModify()) return false;

            var entity = await _candidatoRepository.GetById(id);
            if (entity == null) return false;

            entity.Estado = activar ? EstadoEntidad.Activo : EstadoEntidad.Inactivo;
            var updated = await _candidatoRepository.UpdateAsync(id, entity);
            return updated != null;
        }

        public async Task<bool> CanModify()
        {
            var query = _eleccionRepository.GetAllQuery();
            var hayEleccionActiva = await query.AnyAsync(e => e.Estado == EstadoEleccion.EnProceso);
            return !hayEleccionActiva;
        }

        public async Task<bool> ExisteCandidatoEnPuesto(int puestoId)
        {
            int partidoId = _userSession.GetPartidoId();
            return await _candidatoRepository.ExisteCandidatoEnPuesto(partidoId, puestoId);
        }
    }
}
