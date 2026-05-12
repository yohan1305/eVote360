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
    public class AsignacionCandidatoPuestoConfiguration : IEntityTypeConfiguration<AsignacionCandidatoPuesto>
    {
        public void Configure(EntityTypeBuilder<AsignacionCandidatoPuesto> builder)
        {
            builder.ToTable("AsignacionesCandidatosPuestos");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.FechaAsignacion)
                   .IsRequired();

            builder.HasOne(a => a.Candidato)
                   .WithMany(c => c.Asignaciones)
                   .HasForeignKey(a => a.CandidatoId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.PartidoPolitico)
                   .WithMany(p => p.Asignaciones)
                   .HasForeignKey(a => a.PartidoPoliticoId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.PuestoElectivo)
                   .WithMany(p => p.Asignaciones)
                   .HasForeignKey(a => a.PuestoElectivoId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(a => new { a.CandidatoId, a.PartidoPoliticoId, a.PuestoElectivoId })
                   .IsUnique();
        }
    }
}
