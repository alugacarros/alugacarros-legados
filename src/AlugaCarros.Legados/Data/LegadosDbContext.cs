using AlugaCarros.Legados.Api.Models;
using AlugaCarros.Legados.Api.Models.Agencies;
using AlugaCarros.Legados.Api.Models.Clients;
using AlugaCarros.Legados.Api.Models.Reservations;
using AlugaCarros.Legados.Api.Models.Vehicles;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetDevPack.Security.Jwt.Core.Model;
using NetDevPack.Security.Jwt.Store.EntityFrameworkCore;

namespace AlugaCarros.Legados.Api.Data
{
    public class LegadosDbContext : IdentityDbContext, ISecurityKeyContext
    {
        public LegadosDbContext() { }
        public LegadosDbContext(DbContextOptions<LegadosDbContext> options) : base(options) 
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleGroup> VehicleGroups { get; set; }
        public DbSet<Client> Clients{ get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<KeyMaterial> SecurityKeys { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
           builder.ApplyConfigurationsFromAssembly(typeof(LegadosDbContext).Assembly);

            foreach (var relationship in builder.Model.GetEntityTypes()
                .SelectMany(e => e.GetForeignKeys())) relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;

            base.OnModelCreating(builder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }

}


