using AutoMapper;
using Evote365.Core.Application.Dtos.Elector;
using Evote365.Core.Application.Interfaces.Elector;
using Evote366.Core.Domain.Common.Enums;
using Evote366.Core.Domain.Entities;
using Evote366.Core.Domain.Interfaces;
using CiudadanoDto = Evote365.Core.Application.Dtos.Administrador.Ciudadano.CiudadanoDto;

namespace Evote365.Core.Application.Services.Elector
{
    public class ElectorService : GenericService<Ciudadano, CiudadanoDto>, IElectorService
    {
        private readonly ICiudadanoRepository _ciudadanoRepository;
        private readonly IVotoRepository _votoRepository;
        private readonly IMapper _mapper;

        public ElectorService(
            ICiudadanoRepository ciudadanoRepository,
            IVotoRepository votoRepository,
            IMapper mapper)
            : base(ciudadanoRepository, mapper)
        {
            _ciudadanoRepository = ciudadanoRepository;
            _votoRepository = votoRepository;
            _mapper = mapper;
        }

        public async Task<CiudadanoDto?> ObtenerPorDocumentoAsync(string documento)
        {
            var normalizado = documento.Trim();
            var ciudadano = await _ciudadanoRepository.GetByDocumentoIdentidadAsync(normalizado);
            return ciudadano is null ? null : _mapper.Map<CiudadanoDto>(ciudadano);
        }

        public async Task<bool> EstaActivoAsync(string documento)
        {
            var normalizado = documento.Trim();
            var ciudadano = await _ciudadanoRepository.GetByDocumentoIdentidadAsync(normalizado);
            return ciudadano is not null && ciudadano.Estado == EstadoEntidad.Activo;
        }

        public async Task<bool> ExistePorDocumentoAsync(string documento)
        {
            var normalizado = documento.Trim();
            return await _ciudadanoRepository.ExistsByDocumentoIdentidadAsync(normalizado);
        }

        public async Task<bool> YaHaVotadoAsync(string documento, int eleccionId)
        {
            var ciudadano = await _ciudadanoRepository.GetByDocumentoIdentidadAsync(documento.Trim());
            if (ciudadano is null)
            {
                return false;
            }

            return await _ciudadanoRepository.YaHaVotadoAsync(ciudadano.Id, eleccionId);
        }

        public async Task<List<int>> GetPuestosVotadosAsync(string documento, int eleccionId)
        {
            var ciudadano = await _ciudadanoRepository.GetByDocumentoIdentidadAsync(documento.Trim());
            if (ciudadano is null)
            {
                return [];
            }

            return await _ciudadanoRepository.GetPuestosVotadosAsync(ciudadano.Id, eleccionId);
        }

        public async Task<bool> RegistrarVotoAsync(string documento, int eleccionId, int puestoId, int candidatoId, int partidoId)
        {
            var ciudadano = await _ciudadanoRepository.GetByDocumentoIdentidadAsync(documento.Trim());
            if (ciudadano is null || ciudadano.Estado != EstadoEntidad.Activo)
            {
                return false;
            }

            if (await _votoRepository.ExisteVotoAsync(ciudadano.Id, eleccionId, puestoId))
            {
                return false;
            }

            var voto = new Voto
            {
                CiudadanoId = ciudadano.Id,
                Ciudadano = ciudadano,
                EleccionId = eleccionId,
                Eleccion = null!,
                PuestoElectivoId = puestoId,
                PuestoElectivo = null!,
                CandidatoId = candidatoId,
                PartidoPoliticoId = partidoId,
                FechaEmision = DateTime.Now
            };

            var guardado = await _votoRepository.RegistrarVotoAsync(voto);
            return guardado is not null;
        }

        public async Task<List<VotoResumenItemDto>> ObtenerResumenVotosAsync(string documento, int eleccionId)
        {
            var ciudadano = await _ciudadanoRepository.GetByDocumentoIdentidadAsync(documento.Trim());
            if (ciudadano is null)
            {
                return [];
            }

            var votos = await _votoRepository.GetResumenVotosAsync(ciudadano.Id, eleccionId);
            return votos.Select(v => new VotoResumenItemDto
            {
                Puesto = v.PuestoElectivo.Nombre,
                Candidato = v.Candidato is null ? "No disponible" : $"{v.Candidato.Nombre} {v.Candidato.Apellido}",
                Partido = v.Candidato?.PartidoPolitico?.Siglas ?? "N/D"
            }).ToList();
        }

        public async Task FinalizarVotacionAsync(string documento, int eleccionId)
        {
            var ciudadano = await _ciudadanoRepository.GetByDocumentoIdentidadAsync(documento.Trim());
            if (ciudadano is null)
            {
                return;
            }
        }
    }
}
