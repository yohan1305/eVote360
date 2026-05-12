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
    public class EleccionEntityConfiguration : IEntityTypeConfiguration<Eleccion>
    {
        public void Configure(EntityTypeBuilder<Eleccion> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("Elecciones");
            #endregion

            #region Property configurations
            builder.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            builder.Property(e => e.FechaRealizacion).IsRequired();
            builder.Property(e => e.Estado).IsRequired();
            #endregion

            #region Relationships
            builder.HasMany(e => e.PuestosEnEleccion)
                   .WithOne(ep => ep.Eleccion)
                   .HasForeignKey(ep => ep.EleccionId)
                   .OnDelete(DeleteBehavior.Cascade);
            #endregion
        }
    }
}
