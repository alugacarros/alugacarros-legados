using AlugaCarros.Core.ApiConfiguration;
using AlugaCarros.Legados.Api.Configurations;
using AlugaCarros.Legados.Api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiConfiguration(builder.Configuration);

builder.Configuration.AddUserSecrets<Program>();
builder.AddSerilog("AlugaCarros-Legados");

var app = builder.Build();

app.UseAppConfiguration();

using (var scope = app.Services.CreateScope())
{
    var dataContext = scope.ServiceProvider.GetRequiredService<LegadosDbContext>();
    dataContext.Database.Migrate();
}

app.Run();
