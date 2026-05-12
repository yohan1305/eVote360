using Evote366.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Evote365.Infrastructure.Persistence.EntityConfigurations
{
    public class PuestoElectivoEntityConfiguration : IEntityTypeConfiguration<PuestoElectivo>
    {
        public void Configure(EntityTypeBuilder<PuestoElectivo> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("PuestosElectivos");
            #endregion

            #region Property configurations
            builder.Property(p => p.Nombre).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Estado).IsRequired();
            #endregion

            #region Relationships
            builder.HasMany(p => p.EleccionesAsociadas)
                   .WithOne(e => e.PuestoElectivo)
                   .HasForeignKey(e => e.PuestoElectivoId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Asignaciones)
                   .WithOne(a => a.PuestoElectivo)
                   .HasForeignKey(a => a.PuestoElectivoId)
                   .OnDelete(DeleteBehavior.Restrict);
            #endregion
        }
    }
}
