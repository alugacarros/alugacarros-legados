namespace AlugaCarros.Legados.Api.Models.Vehicles
{
    public class VehicleGroup
    {
        public VehicleGroup(string groupCode, string groupDescription)
        {
            Id = Guid.NewGuid();
            GroupCode = groupCode;
            GroupDescription = groupDescription;
        }

        public Guid Id { get; private set; }
        public string GroupCode { get; private set; }
        public string GroupDescription { get; private set; }
        public IList<Vehicle> Vehicles { get; set; }
    }
}
