using AlugaCarros.Core.Dtos;
using AlugaCarros.Core.ServiceResponse;
using AlugaCarros.Legados.Api.Data;
using AlugaCarros.Legados.Api.Models;
using AlugaCarros.Legados.Api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NetDevPack.Security.Jwt.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace AlugaCarros.Legados.Api.Services;
public class AuthenticationService : IAuthenticationService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly LegadosDbContext _identidadeDbContext;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ILogger<AuthenticationService> _logger;
    private readonly IJwtService _jwtService;
    private readonly IAgencyService _agencyService;
    private readonly IConfiguration _configuration;

    public AuthenticationService(UserManager<IdentityUser> userManager,
        LegadosDbContext identidadeDbContext,
        SignInManager<IdentityUser> signInManager,
        ILogger<AuthenticationService> logger,
        IJwtService jwtService, IAgencyService agencyService, IConfiguration configuration)
    {
        _userManager = userManager;
        _identidadeDbContext = identidadeDbContext;
        _signInManager = signInManager;
        _logger = logger;
        _jwtService = jwtService;
        _agencyService = agencyService;
        this._configuration = configuration;
    }

    public async Task<ResultDto<LoginResponse>> Login(UserLogin userLogin)
    {
        var result = await _signInManager.PasswordSignInAsync(userLogin.Email, userLogin.Password, false, true);
        if(result.IsLockedOut)
            return ResultDto<LoginResponse>.Failed("Login bloqueado. Tente novamente em alguns minutos");

        if (result.Succeeded)
            return ResultDto<LoginResponse>.Success(await GenerateJwt(userLogin.Email), "");

        return ResultDto<LoginResponse>.Failed("Usuário ou senha inválidos");
    }

    public async Task<HttpResultDto> Registry(UserRegistry userRegistry)
    {
        try
        {
            var agencies = _agencyService.GetAgencies();
            if (!userRegistry.Agencies.All(agencyCode => agencies.Any(agency => agency.AgencyCode == agencyCode)))
                return HttpResultDto.Failed(HttpStatusCode.BadRequest, "Agência não existente");

            var usuarioIdentity = GetIdentityUser(userRegistry);
            var result = await _userManager.CreateAsync(usuarioIdentity, userRegistry.Password);

            if (result.Succeeded)
            {
                await SaveUser(userRegistry);
                return HttpResultDto.Success(HttpStatusCode.Created, "");
            }

            return HttpResultDto.Failed(HttpStatusCode.BadRequest,
                $"Houve um problema ao criar o usuário: {string.Join(", ", result.Errors.Select(s => s.Description))}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Ocorreu um erro ao criar o usuário: {ex}");
            return HttpResultDto.Failed(HttpStatusCode.InternalServerError, $"There is an error when creating an user");
        }
    }

    private async Task<LoginResponse> GenerateJwt(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var claims = await _userManager.GetClaimsAsync(user);
        var identityClaims = await GetUserClaims(claims, user);

        var encodedToken = CodificateToken(identityClaims);

        return GetResponseToken(encodedToken, user, claims);
    }


    private string CodificateToken(ClaimsIdentity identityClaims)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = _jwtService.GetCurrentSigningCredentials().Result;
        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = _configuration["ValidIssuer"],
            Subject = identityClaims,
            Expires = DateTime.UtcNow.AddDays(30),
            SigningCredentials = key
        });

        return tokenHandler.WriteToken(token);
    }

    private async Task<ClaimsIdentity> GetUserClaims(ICollection<Claim> claims, IdentityUser user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);

        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(),
            ClaimValueTypes.Integer64));

        foreach (var userRole in userRoles)
        {
            claims.Add(new Claim("role", userRole));
        }

        var identityClaims = new ClaimsIdentity();
        identityClaims.AddClaims(claims);

        return identityClaims;
    }

    private static long ToUnixEpochDate(DateTime date)
    => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
        .TotalSeconds);

    private LoginResponse GetResponseToken(string encodedToken, IdentityUser user,
        IEnumerable<Claim> claims)
    {
        return new LoginResponse
        {
            AccessToken = encodedToken,
            ExpiresIn = TimeSpan.FromDays(30).TotalSeconds,
            UserToken = new UserToken
            {
                Id = user.Id,
                Email = user.Email,
                Claims = claims.Select(c => new UsuarioClaim { Type = c.Type, Value = c.Value })
            }
        };
    }

    private async Task SaveUser(UserRegistry usuarioRegistro)
    {
        var identityUser = await _userManager.FindByEmailAsync(usuarioRegistro.Email);
        if (identityUser != null)
        {
            var claims = usuarioRegistro.Agencies.Select(s => new Claim("agencia", s)).ToList();
            await _userManager.AddClaimsAsync(identityUser, claims);
            _identidadeDbContext.Users.Add(new User(usuarioRegistro.Nome, Guid.Parse(identityUser.Id)));
            _identidadeDbContext.SaveChanges();
        }
    }

    private IdentityUser GetIdentityUser(UserRegistry usuarioRegistro)
    {
        return new IdentityUser
        {
            UserName = usuarioRegistro.Email,
            Email = usuarioRegistro.Email,
            EmailConfirmed = true
        };
    }
}