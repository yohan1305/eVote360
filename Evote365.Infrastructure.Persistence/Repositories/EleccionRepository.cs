using Evote365.Infrastructure.Persistence.Context;
using Evote366.Core.Domain.Common.Enums;
using Evote366.Core.Domain.Entities;
using Evote366.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Evote365.Infrastructure.Persistence.Repositories
{
    public class EleccionRepository : GenericRepository<Eleccion>, IEleccionRepository
    {
        private readonly Evote365DbContext _context;

        public EleccionRepository(Evote365DbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Eleccion?> GetEleccionActiva()
        {
            return await _context.Elecciones
                .Where(e => e.Estado == EstadoEleccion.EnProceso)
                .FirstOrDefaultAsync();
        }

        public async Task<Eleccion?> GetEleccionActivaForElector()
        {
            return await _context.Elecciones
                .Include(e => e.PuestosEnEleccion)
                    .ThenInclude(ep => ep.PuestoElectivo)
                        .ThenInclude(p => p.Asignaciones)
                            .ThenInclude(a => a.Candidato)
                                .ThenInclude(c => c.PartidoPolitico)
                .FirstOrDefaultAsync(e => e.Estado == EstadoEleccion.EnProceso);
        }

        public async Task<List<int>> GetPuestosAsignadosAsync(int eleccionId)
        {
            return await _context.EleccionPuestos
                .Where(ep => ep.EleccionId == eleccionId)
                .Select(ep => ep.PuestoElectivoId)
                .ToListAsync();
        }

        public async Task AsignarPuestosAEleccionAsync(int eleccionId, List<int> puestosIds)
        {
            var existentes = await _context.EleccionPuestos
                .Where(ep => ep.EleccionId == eleccionId)
                .ToListAsync();

            _context.EleccionPuestos.RemoveRange(existentes);

            var nuevas = puestosIds.Select(puestoId => new EleccionPuesto
            {
                EleccionId = eleccionId,
                PuestoElectivoId = puestoId
            });

            await _context.EleccionPuestos.AddRangeAsync(nuevas);
            await _context.SaveChangesAsync();
        }

    }
}
