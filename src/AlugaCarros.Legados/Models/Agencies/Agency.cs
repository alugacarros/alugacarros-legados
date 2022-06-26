namespace AlugaCarros.Legados.Api.Models.Agencies
{
    public class Agency
    {
        public Agency(string agencyCode, string agencyName)
        {
            Id = Guid.NewGuid();
            AgencyCode = agencyCode;
            AgencyName = agencyName;
        }

        public Guid Id { get; private set; }
        public string AgencyCode { get; private set; }
        public string AgencyName { get; private set; }
    }
}
