namespace AlugaCarros.Legados.Api.Models.Vehicles
{
    public class Vehicle
    {
        public Vehicle(Guid idGroup, string agencyCode, VehicleStatus status, string chassi, string plate, string model)
        {
            IdGroup = idGroup;
            AgencyCode = agencyCode;
            Status = status;
            Chassi = chassi;
            Plate = plate;
            Model = model;
        }

        public Guid Id { get; private set; }
        public Guid IdGroup { get; private set; }
        public string AgencyCode { get; private set; }
        public VehicleStatus Status { get; private set; }
        public string Chassi { get; private set; }
        public string Plate { get; private set; }
        public string Model { get; private set; }
        public virtual VehicleGroup Group { get; set; }

        public void RentVeicle() => Status = VehicleStatus.Rented;
    }
}
