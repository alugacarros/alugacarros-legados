using AlugaCarros.Legados.Api.Models;
using AlugaCarros.Legados.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlugaCarros.Legados.Api.Controllers;
[Route("api/v1/users")]
[Authorize]
public class UsersController : Controller
{
    private readonly IAuthenticationService _authenticationService;

    private readonly IConfiguration _configuration;
    public UsersController(IAuthenticationService authenticationService, IConfiguration configuration)
    {
        _configuration = configuration;
        _authenticationService = authenticationService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Login([FromBody] UserLogin usuarioLogin)
    {
        var result = await _authenticationService.Login(usuarioLogin);
        if (result.Fail)
            return StatusCode(400, result.Message);

        return Ok(result.Data);
    }

    [HttpPost("registry")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Registry([FromBody] UserRegistry usuarioRegistro)
    {
        if (!ModelState.IsValid) return BadRequest(new { ErrosValidacao = ModelState.Values.SelectMany(s => s.Errors).Select(s => s.ErrorMessage) });

        var result = await _authenticationService.Registry(usuarioRegistro);
        if (result.Fail)
            return StatusCode((int)result.StatusCode, result.Message);

        return StatusCode(StatusCodes.Status201Created);
    }
}

