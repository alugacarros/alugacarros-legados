using AlugaCarros.Core.Dtos;
using AlugaCarros.Legados.Api.Models.Agencies;

namespace AlugaCarros.Legados.Api.Services.Interfaces
{
    public interface IAgencyService
    {
        List<AgencyRequestResponse> GetAgencies();
        Task<ResultDto> CreateAgency(AgencyRequestResponse createAgency);
    }
}