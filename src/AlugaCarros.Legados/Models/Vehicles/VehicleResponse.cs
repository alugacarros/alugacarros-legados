namespace AlugaCarros.Legados.Api.Models.Vehicles
{
    public class VehicleResponse
    {
        public VehicleResponse(string plate, string model, string vehicleGroup, string vehicleGroupDescription, string agencyCode, VehicleStatus status)
        {
            Plate = plate;
            Model = model;
            VehicleGroup = vehicleGroup;
            VehicleGroupDescription = vehicleGroupDescription;
            AgencyCode = agencyCode;
            Status = status;
        }
        public string AgencyCode { get; set; }
        public VehicleStatus Status { get; set; }
        public string Plate { get; set; }
        public string Model { get; set; }
        public string VehicleGroup { get; set; }
        public string VehicleGroupDescription { get; set; }
    }
}
