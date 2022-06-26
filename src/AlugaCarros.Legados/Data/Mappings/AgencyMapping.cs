using AlugaCarros.Legados.Api.Models.Agencies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlugaCarros.Legados.Api.Data.Mappings
{
    public class AgencyMapping : IEntityTypeConfiguration<Agency>
    {
        public void Configure(EntityTypeBuilder<Agency> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("Agencies");
        }
    }
}
