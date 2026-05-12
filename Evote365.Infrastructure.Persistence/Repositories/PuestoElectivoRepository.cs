using Evote365.Infrastructure.Persistence.Context;
using Evote366.Core.Domain.Common.Enums;
using Evote366.Core.Domain.Entities;
using Evote366.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Evote365.Infrastructure.Persistence.Repositories
{
    public class PuestoElectivoRepository : GenericRepository<PuestoElectivo>, IPuestoElectivoRepository
    {
        private readonly Evote365DbContext _context;
        public PuestoElectivoRepository(Evote365DbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<PuestoElectivo>> GetDisponiblesParaPartido(int partidoId)
        {
            // Solo puestos que NO tengan una asignación ya hecha en TU partido
            var ocupadosIds = await _context.AsignacionesCandidatosPuestos
                .Where(a => a.PartidoPoliticoId == partidoId)
                .Select(a => a.PuestoElectivoId)
                .ToListAsync();

            return await _context.PuestosElectivos
                .Where(p => p.Estado == EstadoEntidad.Activo && !ocupadosIds.Contains(p.Id))
                .ToListAsync();
        }

        public async Task<bool> ExistePuestoActivoAsync(int puestoId)
        {
            return await _context.PuestosElectivos
                .AsNoTracking()
                .AnyAsync(p => p.Id == puestoId && p.Estado == EstadoEntidad.Activo);
        }

        public async Task<PuestoElectivo?> GetByIdAsync(int id)
        {
            return await _context.PuestosElectivos
                .Include(p => p.Candidatos) //para mapeos o verificaciones posteriores
                .Include(p => p.EleccionesAsociadas)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<PuestoElectivo>> GetActivosConAsignacionesAsync()
        {
            return await _context.PuestosElectivos
                .Where(p => p.Estado == EstadoEntidad.Activo)
                .Include(p => p.Asignaciones)
                    .ThenInclude(a => a.Candidato)
                .Include(p => p.Asignaciones)
                    .ThenInclude(a => a.PartidoPolitico)
                .ToListAsync();
        }

        public async Task<List<PuestoElectivo>> GetPuestosActivosAsync()
        {
            return await _context.PuestosElectivos
                .Where(p => p.Estado == EstadoEntidad.Activo)
                .ToListAsync();
        }


    }
}
