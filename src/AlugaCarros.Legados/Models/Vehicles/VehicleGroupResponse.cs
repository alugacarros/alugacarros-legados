namespace AlugaCarros.Legados.Api.Models.Vehicles
{
    public class VehicleGroupResponse
    {
        public VehicleGroupResponse(string code, string description)
        {
            Code = code;
            Description = description;
        }

        public string Code { get; private set; }
        public string Description { get; private set; }
    }
}
