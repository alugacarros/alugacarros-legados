using Bogus;

namespace AlugaCarros.Legados.Api.Models.Reservations;
public class Reservation
{
    public Reservation(Guid idClient, Guid idVehicleGroup, Guid idAgency, DateTime pickupDate, DateTime returnDate)
    {
        Id = Guid.NewGuid();
        IdClient = idClient;
        IdVehicleGroup = idVehicleGroup;
        IdAgency = idAgency;
        PickupDate = pickupDate;
        ReturnDate = returnDate;

        decimal multiplier = ((decimal)new Random().Next(1, 30)) / 100 + 1;
        ReservationValue = Math.Round(((decimal)new Random().Next(150, 200)) * multiplier, 2) * Days;
        SecurityDepositValue = Math.Round(new Random().Next(1500, 2000) * multiplier, 2);
        Status = ReservationStatus.Opened;
        Code = new Faker().Random.AlphaNumeric(8).ToUpper();
    }

    public Guid Id { get; private set; }
    public Guid IdClient { get; private set; }
    public Guid IdVehicleGroup { get; private set; }
    public Guid IdAgency { get; private set; }
    public DateTime PickupDate { get; private set; }
    public DateTime ReturnDate { get; private set; }
    public string Code { get; private set; }
    public decimal ReservationValue { get; private set; }
    public decimal SecurityDepositValue { get; private set; }
    public ReservationStatus Status { get; private set; }
    public int Days => ReturnDate.DayOfYear - PickupDate.DayOfYear;

    public void CloseReservation() => Status = ReservationStatus.Closed;
}

public enum ReservationStatus
{
    Opened = 1,
    Closed = 2,
    Canceled = 3
}