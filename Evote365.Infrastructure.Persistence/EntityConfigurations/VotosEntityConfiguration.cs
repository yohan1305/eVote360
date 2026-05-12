using Evote366.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evote365.Infrastructure.Persistence.EntityConfigurations
{
    public class VotoEntityConfiguration : IEntityTypeConfiguration<Voto>
    {
        public void Configure(EntityTypeBuilder<Voto> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("Votos");
            #endregion

            #region Property configurations
            builder.Property(v => v.FechaEmision).IsRequired();
            #endregion

            #region Relationships
            builder.HasOne(v => v.Ciudadano)
                   .WithMany(c => c.VotosEmitidos)
                   .HasForeignKey(v => v.CiudadanoId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(v => v.Eleccion)
         .WithMany(e => e.VotosEmitidos)
         .HasForeignKey(v => v.EleccionId)
         .OnDelete(DeleteBehavior.Cascade);


            builder.HasOne(v => v.PuestoElectivo)
                   .WithMany()
                   .HasForeignKey(v => v.PuestoElectivoId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(v => v.Candidato)
           .WithMany(c => c.VotosRecibidos)
           .HasForeignKey(v => v.CandidatoId)
           .OnDelete(DeleteBehavior.SetNull);
            #endregion
        }
    }
}
