using AlugaCarros.Legados.Api.Models.Agencies;
using AlugaCarros.Legados.Api.Models.Clients;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AlugaCarros.Legados.Api.Data.Mappings
{
    public class ClientMapping : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(x => x.Id);

            builder.ToTable("Clients");
        }
    }
}
