using Evote366.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Evote365.Infrastructure.Persistence.EntityConfigurations
{
    public class CiudadanoEntityConfiguration : IEntityTypeConfiguration<Ciudadano>
    {
        public void Configure(EntityTypeBuilder<Ciudadano> builder)
        {
            #region Basic configuration
            builder.HasKey(x => x.Id);
            builder.ToTable("Ciudadanos");
            #endregion

            #region Property configurations
            builder.Property(c => c.Nombre).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Apellido).IsRequired().HasMaxLength(100);
            builder.Property(c => c.DocumentoIdentidad).IsRequired().HasMaxLength(20);
            builder.Property(c => c.Estado).IsRequired();
            builder.HasIndex(c => c.DocumentoIdentidad).IsUnique();
            #endregion

            #region Relationships
            builder.HasMany(c => c.VotosEmitidos)
                   .WithOne(v => v.Ciudadano)
                   .HasForeignKey(v => v.CiudadanoId)
                   .OnDelete(DeleteBehavior.Cascade);
            #endregion
        }
    }

}
