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
    public class EleccionPuestoEntityConfiguration : IEntityTypeConfiguration<EleccionPuesto>
    {
        public void Configure(EntityTypeBuilder<EleccionPuesto> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("EleccionPuestos");
            #endregion

            #region Relationships
            builder.HasOne(ep => ep.Eleccion)
                   .WithMany(e => e.PuestosEnEleccion)
                   .HasForeignKey(ep => ep.EleccionId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ep => ep.PuestoElectivo)
                   .WithMany(pe => pe.EleccionesAsociadas)
                   .HasForeignKey(ep => ep.PuestoElectivoId)
                   .OnDelete(DeleteBehavior.Cascade);
            #endregion
        }
    }
}
