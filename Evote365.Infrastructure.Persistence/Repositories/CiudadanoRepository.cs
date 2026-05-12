using Evote365.Infrastructure.Persistence.Context;
using Evote366.Core.Domain.Entities;
using Evote366.Core.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Evote365.Infrastructure.Persistence.Repositories
{
    public class CiudadanoRepository : GenericRepository<Ciudadano>, ICiudadanoRepository
    {
        private readonly Evote365DbContext _context;

        public CiudadanoRepository(Evote365DbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Ciudadano?> GetByCedulaAsync(string cedula)
        {
            return await _context.Ciudadanos
                .FirstOrDefaultAsync(c => c.DocumentoIdentidad == cedula);
        }

        public async Task<Ciudadano?> GetByDocumentoIdentidadAsync(string documento)
        {
            if (string.IsNullOrWhiteSpace(documento)) return null;

            // 1. Limpiamos el parámetro que viene del usuario (quitamos espacios y guiones)
            var cedulaLimpia = documento.Replace("-", "").Trim();

            // 2. Buscamos comparando la columna formateada vs el parámetro limpio
            return await _context.Ciudadanos
                .FirstOrDefaultAsync(c => c.DocumentoIdentidad.Replace("-", "") == cedulaLimpia);
        }

        public async Task<bool> ExistsByDocumentoIdentidadAsync(string documento)
        {
            var normalizado = documento.Trim();
            return await _context.Ciudadanos
                .AnyAsync(c => c.DocumentoIdentidad == normalizado);
        }

        public async Task<bool> YaHaVotadoAsync(int ciudadanoId, int eleccionId)
        {
            return await _context.Votos
                .AnyAsync(v => v.CiudadanoId == ciudadanoId && v.EleccionId == eleccionId);
        }

        public async Task<List<int>> GetPuestosVotadosAsync(int ciudadanoId, int eleccionId)
        {
            return await _context.Votos
                .Where(v => v.CiudadanoId == ciudadanoId && v.EleccionId == eleccionId)
                .Select(v => v.PuestoElectivoId)
                .Distinct()
                .ToListAsync();
        }

        public async Task MarcarVotacionFinalizadaAsync(int ciudadanoId)
        {
            var ciudadano = await _context.Ciudadanos.FindAsync(ciudadanoId);
            if (ciudadano is null) return;

            ciudadano.YaVoto = true;
            _context.Ciudadanos.Update(ciudadano);
            await _context.SaveChangesAsync();
        }

    }
}
