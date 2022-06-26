using AlugaCarros.Core.ApiConfiguration;
using AlugaCarros.Core.Middlewares;
using AlugaCarros.Legados.Api.Data;
using AlugaCarros.Legados.Api.Extensions;
using AlugaCarros.Legados.Api.Services;
using AlugaCarros.Legados.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetDevPack.Security.JwtExtensions;

namespace AlugaCarros.Legados.Api.Configurations;
public static class ApiConfiguration
{
    public static IServiceCollection AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDefaultApiConfiguration(configuration);

        services.RegistryDependencies(configuration);

        return services;
    }
    private static IServiceCollection RegistryDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        var serverVersion = new MySqlServerVersion(new Version(8, 0, 0));
        var connectionString = $"Server={configuration["MySqlHost"]};Port=3306;Database=alugacarros-legados;Uid={configuration["MySqlUser"]};Pwd={configuration["MySqlPass"]};";
        services.AddDbContext<LegadosDbContext>(options =>
        options.UseMySql(connectionString, serverVersion));

        services.AddDefaultIdentity<IdentityUser>()
            .AddRoles<IdentityRole>()
            .AddErrorDescriber<IdentityPortugueseMessages>()
            .AddEntityFrameworkStores<LegadosDbContext>()
            .AddDefaultTokenProviders();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddTransient<IAuthenticationService, AuthenticationService>();
        services.AddTransient<IAgencyService, AgencyService>();
        services.AddTransient<IVehiclesService, VehiclesService>();
        services.AddTransient<IReservationsService, ReservationsService>();

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.BackchannelHttpHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = delegate { return true; } };
            x.SaveToken = true;
            x.SetJwksOptions(new JwkOptions(configuration["AuthenticationJwksUrl"]));
            x.TokenValidationParameters.ValidIssuer = configuration["ValidIssuer"];
        });

        services.AddJwksManager().PersistKeysToDatabaseStore<LegadosDbContext>();

        return services;
    }

    public static WebApplication UseAppConfiguration(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseJwksDiscovery();

        app.UseMiddleware<ErrorHandlingMiddleware>();
        app.UseMiddleware<LoggingRequestMiddleware>();

        app.MapControllers();

        app.UseCors("Total");

        return app;
    }
}