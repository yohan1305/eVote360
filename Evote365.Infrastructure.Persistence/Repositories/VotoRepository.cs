using Evote365.Infrastructure.Persistence.Context;
using Evote366.Core.Domain.Entities;
using Evote366.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Evote365.Infrastructure.Persistence.Repositories
{
    public class VotoRepository : GenericRepository<Voto>, IVotoRepository
    {
        private readonly Evote365DbContext _context;

        public VotoRepository(Evote365DbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExisteVotoAsync(int ciudadanoId, int eleccionId, int puestoElectivoId)
        {
            return await _context.Votos.AnyAsync(v =>
                v.CiudadanoId == ciudadanoId &&
                v.EleccionId == eleccionId &&
                v.PuestoElectivoId == puestoElectivoId);
        }

        public async Task<Voto?> RegistrarVotoAsync(Voto voto)
        {
            var entry = await _context.Votos.AddAsync(voto);
            await _context.SaveChangesAsync();
            return entry.Entity;
        }

        public async Task<List<Voto>> GetResumenVotosAsync(int ciudadanoId, int eleccionId)
        {
            return await _context.Votos
                .Include(v => v.PuestoElectivo)
                .Include(v => v.Candidato)
                    .ThenInclude(c => c!.PartidoPolitico)
                .Where(v => v.CiudadanoId == ciudadanoId && v.EleccionId == eleccionId)
                .OrderBy(v => v.PuestoElectivo.Nombre)
                .ToListAsync();
        }
    }
}
