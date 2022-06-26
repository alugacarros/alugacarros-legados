using AlugaCarros.Legados.Api.Models.Vehicles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlugaCarros.Legados.Api.Data.Mappings
{
    public class VehicleMapping : IEntityTypeConfiguration<Vehicle>
    {
        public void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Group)
                   .WithMany(m => m.Vehicles)
                   .HasForeignKey(x => x.IdGroup);

            builder.ToTable("Vehicles");
        }
    }
}
