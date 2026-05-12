using Evote365.Infrastructure.Persistence.Context;
using Evote366.Core.Domain.Common.Enums;
using Evote366.Core.Domain.Entities;
using Evote366.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Evote365.Infrastructure.Persistence.Repositories
{
    public class CandidatoRepository : GenericRepository<Candidato>, ICandidatoRepository
    {
        private readonly Evote365DbContext _context;

        public CandidatoRepository(Evote365DbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Candidato>> GetByPartidoIdWithInclude(int partidoId)
        {
            return await _context.Candidatos
                .Include(c => c.PuestoElectivo)
                .Include(c => c.PartidoPolitico)
                .Where(c => c.PartidoPoliticoId == partidoId)
                .ToListAsync();
        }

        public async Task<List<Candidato>> GetCandidatosPorPartidoYAliadosAsync(List<int> partidoYAliadosIds)
        {
            return await _context.Candidatos
                .Include(c => c.Asignaciones) // REQUERIDO: Para que SelectMany funcione
                    .ThenInclude(a => a.PuestoElectivo) // Opcional: Pero bueno por si acaso
                .Where(c => c.Estado == EstadoEntidad.Activo &&
                            partidoYAliadosIds.Contains(c.PartidoPoliticoId))
                .ToListAsync();
        }

        public async Task<bool> UpdatePuestoElectivoAsync(int candidatoId, int nuevoPuestoId)
        {
            var candidato = await _context.Candidatos
                .AsTracking()
                .FirstOrDefaultAsync(c => c.Id == candidatoId);

            if (candidato == null)
                return false;

            // Asignar el nuevo puesto
            candidato.PuestoElectivoId = nuevoPuestoId;

            // Forzar que EF lo marque como modificado
            _context.Entry(candidato).Property(c => c.PuestoElectivoId).IsModified = true;

            // Guardar cambios
            var resultado = await _context.SaveChangesAsync();
            return resultado > 0;
        }
        public async Task<bool> ExisteCandidatoEnPuesto(int partidoId, int puestoId)
        {
            return await _context.Candidatos
                .AnyAsync(c => c.PartidoPoliticoId == partidoId && c.PuestoElectivoId == puestoId);
        }

        public async Task<List<Candidato>> GetDisponiblesPorPartido(int partidoId)
        {
            return await _context.Candidatos
          .Include(c => c.PartidoPolitico)
               .Where(c => c.PartidoPoliticoId == partidoId &&
                c.Estado == EstadoEntidad.Activo &&
                c.PuestoElectivoId == null)
               .ToListAsync();
        }

        public async Task<List<Candidato>> GetDisponiblesPorPartidos(List<int> partidoIds)
        {
            return await _context.Candidatos
                .Include(c => c.PartidoPolitico)
                .Include(c => c.PuestoElectivo)
                .Where(c =>
                    partidoIds.Contains(c.PartidoPoliticoId) &&
                    c.Estado == EstadoEntidad.Activo &&
                    c.PuestoElectivoId == null)
                .ToListAsync();
        }

        public async Task<List<Candidato>> GetAliadosConPuesto(int puestoId, List<int> aliadosIds)
        {
            return await _context.Candidatos
                .Where(c => aliadosIds.Contains(c.PartidoPoliticoId) &&
                            c.Estado == EstadoEntidad.Activo &&
                            c.PuestoElectivoId == puestoId)
                .ToListAsync();
        }



        public async Task<List<Candidato>> GetAliadosSinPuesto(List<int> aliadosIds)
        {
            return await _context.Candidatos
                .Where(c => aliadosIds.Contains(c.PartidoPoliticoId) &&
                            c.Estado == EstadoEntidad.Activo &&
                            c.PuestoElectivoId == null)
                .ToListAsync();
        }

        public async Task<List<Candidato>> GetPorPuesto(int puestoId)
        {
            return await _context.Candidatos
                .Where(c => c.PuestoElectivoId == puestoId)
                .ToListAsync();
        }


        public async Task<Candidato?> GetByIdWithInclude(int id)
        {
            return await _context.Candidatos
                .Include(c => c.PuestoElectivo)
                .Include(c => c.PartidoPolitico)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Candidato?> GetByIdAsync(int id)
        {
            return await _context.Candidatos
                .Include(c => c.PartidoPolitico)
                .FirstOrDefaultAsync(c => c.Id == id);
        }


        public async Task<List<Candidato>> GetPorPartidosConInclude(List<int> partidoIds)
        {
            return await _context.Candidatos
                .Include(c => c.PartidoPolitico)
                .Include(c => c.PuestoElectivo)
                .Where(c => partidoIds.Contains(c.PartidoPoliticoId) && c.Estado == EstadoEntidad.Activo)
                .ToListAsync();
        }

        public async Task<List<Candidato>> GetCandidatosDisponiblesParaAsignacion(int partidoId, List<int> aliadosIds, int puestoElectivoId)
        {
            // 1. Propios: Activos, de mi partido y que no tengan NINGUNA asignación en mi partido
            var propios = await _context.Candidatos
                .Include(c => c.PartidoPolitico)
                .Where(c => c.PartidoPoliticoId == partidoId &&
                            c.Estado == EstadoEntidad.Activo &&
                            !_context.AsignacionesCandidatosPuestos.Any(a => a.CandidatoId == c.Id && a.PartidoPoliticoId == partidoId))
                .ToListAsync();

            // 2. Aliados: Solo si en su partido de origen tienen EXACTAMENTE el mismo puestoId
            var aliadosValidos = await _context.AsignacionesCandidatosPuestos
                .Include(a => a.Candidato).ThenInclude(c => c.PartidoPolitico)
                .Where(a => aliadosIds.Contains(a.PartidoPoliticoId) &&
                            a.PuestoElectivoId == puestoElectivoId &&
                            a.Candidato.Estado == EstadoEntidad.Activo &&
                            // Y que yo no lo haya asignado ya a nada en mi partido
                            !_context.AsignacionesCandidatosPuestos.Any(ap => ap.CandidatoId == a.CandidatoId && ap.PartidoPoliticoId == partidoId))
                .Select(a => a.Candidato)
                .ToListAsync();

            return propios.Concat(aliadosValidos).ToList();
        }
        public async Task<bool> CandidatoYaAsignadoEnPartido(int candidatoId, int partidoId)
        {
            return await _context.Candidatos
                .AnyAsync(c => c.Id == candidatoId && c.PartidoPoliticoId == partidoId && c.PuestoElectivoId != null);
        }

        public async Task<bool> CandidatoAliadoAspiraAlMismoPuesto(int candidatoId, int puestoId)
        {
            var candidato = await _context.Candidatos
                .Include(c => c.PuestoElectivo)
                .FirstOrDefaultAsync(c => c.Id == candidatoId);

            return candidato?.PuestoElectivoId == puestoId;
        }

        public async Task<List<Candidato>> GetAsignacionesConPuestoPorPartido(int partidoId)
        {
            return await _context.Candidatos
                .Include(c => c.PartidoPolitico)
                .Include(c => c.PuestoElectivo)
                .Where(c => c.PartidoPoliticoId == partidoId && c.PuestoElectivoId != null)
                .ToListAsync();
        }

    }

}
