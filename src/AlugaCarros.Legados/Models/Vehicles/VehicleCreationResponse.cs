namespace AlugaCarros.Legados.Api.Models.Vehicles
{
    public class VehicleCreationResponse
    {
        public VehicleCreationResponse(string plate, string model)
        {
            Plate = plate;
            Model = model;
        }

        public string Plate { get; set; }
        public string Model { get; set; }
    }
}
