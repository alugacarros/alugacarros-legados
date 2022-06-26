using AlugaCarros.Core.Dtos;
using AlugaCarros.Legados.Api.Models.Vehicles;

namespace AlugaCarros.Legados.Api.Services.Interfaces;
public interface IVehiclesService
{
    ResultDto<List<VehicleResponse>> GetVehiclesByAgency(string agencyCode);
    ResultDto<VehicleResponse> GetVehiclesByPlate(string plate);
    ResultDto<List<VehicleGroupResponse>> GetVehicleGroups();
    ResultDto<VehicleCreationResponse> CreateVehicle(string groupCode, string agencyCode);
    ResultDto RentVehicle(string plate);
}