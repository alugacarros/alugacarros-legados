using AlugaCarros.Legados.Api.Models.Reservations;
using AlugaCarros.Legados.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlugaCarros.Legados.Api.Controllers;
[Route("api/v1/reservations")]
[Authorize]
public class ReservationsController : Controller
{
    private readonly IReservationsService _reservationsService;

    public ReservationsController(IReservationsService reservationsService)
    {
        _reservationsService = reservationsService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public IActionResult CreateReservation([FromBody] CreateReservationRequest request)
    {
        var response = _reservationsService.GenerateReservation(
            request.ClientDocument, request.VehicleGroupCode, request.AgencyCode, request.PickupDate, request.ReturnDate);

        return response.Fail ? BadRequest(response.Message) : StatusCode(201, response.Message);
    }
    [HttpGet("agency/{agencyCode}")]
    [ProducesResponseType(typeof(List<ReservationsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<ReservationsResponse>), StatusCodes.Status404NotFound)]
    public IActionResult GetReservations(string agencyCode)
    {
        var reservations = _reservationsService.GetReservations(agencyCode);

        return reservations.Any() ? StatusCode(200, reservations) : StatusCode(404, reservations);
    }

    [HttpGet("{reservationCode}")]
    [ProducesResponseType(typeof(ReservationsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ReservationsResponse), StatusCodes.Status404NotFound)]
    public IActionResult GetReservation(string reservationCode)
    {
        var reservation = _reservationsService.GetReservation(reservationCode);

        return reservation is not null ? StatusCode(200, reservation) : StatusCode(404, reservation);
    }

    [HttpPost("{reservationCode}/close")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public IActionResult CloseReservation([FromRoute] string reservationCode)
    {
        var response = _reservationsService.CloseReservation(reservationCode);

        return StatusCode(response.Fail ? 400 : 200, response.Message);
    }
}


