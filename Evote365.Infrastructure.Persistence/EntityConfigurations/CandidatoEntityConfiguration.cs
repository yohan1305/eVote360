using Evote366.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evote365.Infrastructure.Persistence.EntityConfigurations
{
    public class CandidatoEntityConfiguration : IEntityTypeConfiguration<Candidato>
    {
        public void Configure(EntityTypeBuilder<Candidato> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("Candidatos");
            #endregion

            #region Property configurations
            builder.Property(c => c.Nombre).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Apellido).IsRequired().HasMaxLength(100);
            builder.Property(c => c.FotoUrl).HasMaxLength(255);
            builder.Property(c => c.Estado).IsRequired();
            #endregion

            #region Relationships
            builder.HasOne(c => c.PartidoPolitico)
                   .WithMany(p => p.Candidatos)
                   .HasForeignKey(c => c.PartidoPoliticoId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(c => c.PuestoElectivo)
                   .WithMany(p => p.Candidatos)
                   .HasForeignKey(c => c.PuestoElectivoId)
                   .OnDelete(DeleteBehavior.SetNull);
            #endregion
        }
    }
}
