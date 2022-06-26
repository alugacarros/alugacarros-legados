using AlugaCarros.Core.Dtos;
using AlugaCarros.Core.ServiceResponse;
using AlugaCarros.Legados.Api.Models;

namespace AlugaCarros.Legados.Api.Services.Interfaces;
public interface IAuthenticationService
{
    Task<ResultDto<LoginResponse>> Login(UserLogin userLogin);
    Task<HttpResultDto> Registry(UserRegistry userRegistry);
}