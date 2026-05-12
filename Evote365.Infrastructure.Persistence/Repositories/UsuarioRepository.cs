using Evote365.Infrastructure.Persistence.Context;
using Evote366.Core.Domain.Entities;
using Evote366.Core.Domain.Interfaces;

namespace Evote365.Infrastructure.Persistence.Repositories
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        private readonly Evote365DbContext _context;
        public UsuarioRepository(Evote365DbContext context) : base(context)
        {
            _context = context;
        }


        public async Task AsignarPartido(int usuarioId, int partidoId)
        {
            var usuario = await _context.Usuarios.FindAsync(usuarioId);
            if (usuario != null)
            {
                usuario.PartidoAsignadoId = partidoId;
                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();
            }
        }

    }
}
