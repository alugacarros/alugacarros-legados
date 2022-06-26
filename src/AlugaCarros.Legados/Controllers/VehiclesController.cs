using AlugaCarros.Legados.Api.Models.Vehicles;
using AlugaCarros.Legados.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlugaCarros.Legados.Api.Controllers;
[Route("api/v1/vehicles")]
[Authorize]
public class VehiclesController : Controller
{
    private readonly IVehiclesService _vehiclesService;

    public VehiclesController(IVehiclesService vehiclesService)
    {
        _vehiclesService = vehiclesService;
    }

    [HttpGet("{vehiclePlate}")]
    [ProducesResponseType(typeof(VehicleResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(VehicleResponse), StatusCodes.Status404NotFound)]
    public IActionResult GetVehiclesByPlate(string vehiclePlate)
    {
        var vehiclesResponse = _vehiclesService.GetVehiclesByPlate(vehiclePlate);
        if (vehiclesResponse.Fail) return NotFound(vehiclesResponse.Message);

        return Ok(vehiclesResponse.Data);
    }

    [HttpGet("agency/{agencyCode}")]
    [ProducesResponseType(typeof(List<VehicleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<VehicleResponse>), StatusCodes.Status404NotFound)]
    public IActionResult GetVehiclesByAgency(string agencyCode)
    {
        var vehiclesResponse = _vehiclesService.GetVehiclesByAgency(agencyCode);
        if (vehiclesResponse.Fail) return BadRequest(vehiclesResponse.Message);

        return vehiclesResponse.Data.Any() ? Ok(vehiclesResponse.Data) : NotFound(new List<VehicleResponse>());
    }

    [HttpGet("groups")]
    [ProducesResponseType(typeof(List<VehicleGroupResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<VehicleGroupResponse>), StatusCodes.Status400BadRequest)]
    public IActionResult GetVehicleGroups()
    {
        var groups = _vehiclesService.GetVehicleGroups();
        if (groups.Fail) return BadRequest(groups.Message);

        return Ok(groups.Data);
    }

    [HttpPost]
    [ProducesResponseType(typeof(VehicleCreationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public IActionResult CreateVehicle([FromBody] VehicleCreationRequest request)
    {
        var vehicleResponse = _vehiclesService.CreateVehicle(request.GroupCode, request.AgencyCode);

        if (vehicleResponse.Fail) return BadRequest(vehicleResponse.Message);

        return Ok(vehicleResponse.Data);
    }

    [HttpPost("{plate}/rent")]
    [ProducesResponseType(typeof(VehicleCreationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public IActionResult RentVehicle([FromRoute] string plate)
    {
        var vehicleResponse = _vehiclesService.RentVehicle(plate);

        return StatusCode(vehicleResponse.Fail ? 400 : 200, vehicleResponse.Message);
    }
}


