using AlugaCarros.Core.Dtos;
using AlugaCarros.Legados.Api.Data;
using AlugaCarros.Legados.Api.Models.Vehicles;
using AlugaCarros.Legados.Api.Services.Interfaces;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace AlugaCarros.Legados.Api.Services;

public class VehiclesService : IVehiclesService
{
    private readonly LegadosDbContext _dbContext;

    public VehiclesService(LegadosDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public ResultDto<List<VehicleResponse>> GetVehiclesByAgency(string agencyCode)
    {
        var agency = GetAgency(agencyCode);

        if (agency == null) return ResultDto<List<VehicleResponse>>.Failed(AgencyNotFoundMessage(agencyCode));

        var vehicles = _dbContext.Vehicles.Where(w => w.AgencyCode == agency.AgencyCode)
                                                      .Include(i => i.Group)
                                                      .Select(s => new VehicleResponse(s.Plate, s.Model, s.Group.GroupCode,
                                                        s.Group.GroupDescription, s.AgencyCode, s.Status))
                                                      .ToList();

        return ResultDto<List<VehicleResponse>>.Success(vehicles, "");
    }

    public ResultDto<VehicleResponse> GetVehiclesByPlate(string plate)
    {
        var vehicle = _dbContext.Vehicles.Where(w => w.Plate == plate)
                                                      .Include(i => i.Group)
                                                      .Select(s => new VehicleResponse(s.Plate, s.Model, 
                                                                    s.Group.GroupCode, s.Group.GroupDescription, 
                                                                    s.AgencyCode, s.Status))
                                                      .FirstOrDefault();
        if (vehicle == null) return ResultDto<VehicleResponse>.Failed("Veículo não encontrado!");

        return ResultDto<VehicleResponse>.Success(vehicle, "");
    }

    private static string AgencyNotFoundMessage(string agencyCode)
    {
        return $"Agência não encontrada para o código {agencyCode}";
    }

    private Models.Agencies.Agency? GetAgency(string agencyCode)
    {
        return _dbContext.Agencies.FirstOrDefault(ag => ag.AgencyCode == agencyCode);
    }

    public ResultDto<List<VehicleGroupResponse>> GetVehicleGroups()
    {
        var groups = _dbContext.VehicleGroups
            .Select(s => new VehicleGroupResponse(s.GroupCode, s.GroupDescription))
            .ToList();

        return ResultDto<List<VehicleGroupResponse>>.Success(groups, "");
    }

    public ResultDto<VehicleCreationResponse> CreateVehicle(string groupCode, string agencyCode)
    {
        var agency = GetAgency(agencyCode);

        if (agency == null) return ResultDto<VehicleCreationResponse>.Failed(AgencyNotFoundMessage(agencyCode));

        var group = _dbContext.VehicleGroups.FirstOrDefault(ag => ag.GroupCode == groupCode);

        if (group == null) return ResultDto<VehicleCreationResponse>.Failed($"Grupo não encontrada para o código {groupCode}");

        var faker = new Faker();

        var vehicle = new Vehicle(group.Id, agency.AgencyCode, VehicleStatus.Available,
            faker.Vehicle.Vin().ToUpper(), 
            faker.Vehicle.Random.AlphaNumeric(7).ToUpper(), 
            faker.Vehicle.Model().ToUpper());

        _dbContext.Vehicles.Add(vehicle);

        _dbContext.SaveChanges();

        return ResultDto<VehicleCreationResponse>.Success(new VehicleCreationResponse(vehicle.Plate, vehicle.Model), "");
    }

    public ResultDto RentVehicle(string plate)
    {
        var vehicle = _dbContext.Vehicles.FirstOrDefault(f => f.Plate == plate);

        if (vehicle == null) return ResultDto.Failed("Veículo não encontrado");

        vehicle.RentVeicle();

        _dbContext.Vehicles.Update(vehicle);

        _dbContext.SaveChanges();

        return ResultDto.Success("Veículo Alugado com Sucesso!");
    }
}
