using AlugaCarros.Legados.Api.Models.Reservations;
using AlugaCarros.Legados.Api.Models.Vehicles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlugaCarros.Legados.Api.Data.Mappings
{
    public class ReservationMapping : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.ToTable("Reservations");
        }
    }
}
