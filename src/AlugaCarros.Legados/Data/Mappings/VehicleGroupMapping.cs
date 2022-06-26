using AlugaCarros.Legados.Api.Models.Vehicles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlugaCarros.Legados.Api.Data.Mappings
{
    public class VehicleGroupMapping : IEntityTypeConfiguration<VehicleGroup>
    {
        public void Configure(EntityTypeBuilder<VehicleGroup> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasDefaultValueSql("(UUID())");

            builder.HasMany(x => x.Vehicles)
                   .WithOne(m => m.Group)
                   .HasForeignKey(x => x.IdGroup);

            builder.ToTable("VehicleGroups");
        }
    }
}
