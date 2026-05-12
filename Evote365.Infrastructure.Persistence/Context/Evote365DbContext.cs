using Evote366.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Evote365.Infrastructure.Persistence.Context
{
    public class Evote365DbContext : DbContext
    {
        public Evote365DbContext(DbContextOptions<Evote365DbContext> options) : base(options) { }

        public DbSet<Ciudadano> Ciudadanos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<PartidoPolitico> PartidosPoliticos { get; set; }
        public DbSet<PuestoElectivo> PuestosElectivos { get; set; }
        public DbSet<Candidato> Candidatos { get; set; }
        public DbSet<Eleccion> Elecciones { get; set; }
        public DbSet<Voto> Votos { get; set; }
        public DbSet<AlianzaPolitica> AlianzasPoliticas { get; set; }

        public DbSet<AsignacionCandidatoPuesto> AsignacionesCandidatosPuestos { get; set; }
        public DbSet<EleccionPuesto> EleccionPuestos { get; set; }

        public DbSet<DirigentePartido> DirigentesPartidos { get; set; }

       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Liskov-substitution

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
}
