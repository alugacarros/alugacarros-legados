using AlugaCarros.Legados.Api.Models.Agencies;
using AlugaCarros.Legados.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlugaCarros.Legados.Api.Controllers;
[Route("api/v1/agencies")]
[Authorize]
public class AgenciesController : Controller
{
    private readonly IAgencyService _agencyService;

    public AgenciesController(IAgencyService agencyService)
    {
        _agencyService = agencyService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateAgency([FromBody] AgencyRequestResponse createAgency)
    {
        var response = await _agencyService.CreateAgency(createAgency);
        return response.Fail ? StatusCode(StatusCodes.Status400BadRequest, response.Message) : StatusCode(StatusCodes.Status201Created);
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<AgencyRequestResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<AgencyRequestResponse>), StatusCodes.Status404NotFound)]
    public ActionResult GetAgencies()
    {
        var response = _agencyService.GetAgencies();
        return StatusCode(response.Any() ? StatusCodes.Status200OK : StatusCodes.Status404NotFound, response);
    }

    [HttpGet("{codigoAgencia}")]
    [ProducesResponseType(typeof(AgencyRequestResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AgencyRequestResponse), StatusCodes.Status404NotFound)]
    public ActionResult GetAgency(string codigoAgencia)
    {
        var response = _agencyService.GetAgencies().FirstOrDefault(w => w.AgencyCode == codigoAgencia);
        return StatusCode(response != null ? StatusCodes.Status200OK : StatusCodes.Status404NotFound, response);
    }

}

