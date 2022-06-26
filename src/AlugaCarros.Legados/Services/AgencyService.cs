using AlugaCarros.Core.Dtos;
using AlugaCarros.Legados.Api.Data;
using AlugaCarros.Legados.Api.Models.Agencies;
using AlugaCarros.Legados.Api.Services.Interfaces;

namespace AlugaCarros.Legados.Api.Services
{
    public class AgencyService : IAgencyService
    {
        private readonly LegadosDbContext _DbContext;

        public AgencyService(LegadosDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<ResultDto> CreateAgency(AgencyRequestResponse createAgency)
        {
            if (_DbContext.Agencies.Any(a => a.AgencyCode == createAgency.AgencyCode))
                return ResultDto.Failed($"Já existe uma agência cadastrada com o código {createAgency.AgencyCode}");

            _DbContext.Agencies.Add(new Agency(createAgency.AgencyCode, createAgency.AgencyName));

            await _DbContext.SaveChangesAsync();

            return ResultDto.Success();
        }

        public List<AgencyRequestResponse> GetAgencies()
        {
            return _DbContext.Agencies.Select(s => 
            new AgencyRequestResponse() 
            { 
                AgencyCode = s.AgencyCode, 
                AgencyName = s.AgencyName 
            }).ToList();

        }
    }
}
