namespace AlugaCarros.Legados.Api.Models.Reservations;

public class CreateReservationRequest
{
    public string ClientDocument { get; set; }
    public string VehicleGroupCode { get; set; }
    public string AgencyCode { get; set; }
    public DateTime PickupDate { get; set; }
    public DateTime ReturnDate { get; set; }
}