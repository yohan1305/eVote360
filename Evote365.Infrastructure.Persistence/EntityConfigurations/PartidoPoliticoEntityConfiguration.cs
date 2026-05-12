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
    public class PartidoPoliticoEntityConfiguration : IEntityTypeConfiguration<PartidoPolitico>
    {
        public void Configure(EntityTypeBuilder<PartidoPolitico> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("PartidosPoliticos");
            #endregion

            #region Property configurations
            builder.Property(p => p.Nombre).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Siglas).IsRequired().HasMaxLength(10);
            builder.HasIndex(p => p.Siglas).IsUnique();
            builder.Property(p => p.Estado).IsRequired();
            #endregion

            #region Relationships
            builder.HasMany(p => p.Dirigentes)
                   .WithOne(u => u.PartidoAsignado)
                   .HasForeignKey(u => u.PartidoAsignadoId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(p => p.Candidatos)
                   .WithOne(c => c.PartidoPolitico)
                   .HasForeignKey(c => c.PartidoPoliticoId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.AlianzasEnviadas)
                   .WithOne(a => a.PartidoSolicitante)
                   .HasForeignKey(a => a.PartidoSolicitanteId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.AlianzasRecibidas)
                   .WithOne(a => a.PartidoReceptor)
                   .HasForeignKey(a => a.PartidoReceptorId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Asignaciones)
                   .WithOne(a => a.PartidoPolitico)
                   .HasForeignKey(a => a.PartidoPoliticoId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
