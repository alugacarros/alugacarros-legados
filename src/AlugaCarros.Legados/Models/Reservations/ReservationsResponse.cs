namespace AlugaCarros.Legados.Api.Models.Reservations
{
    public class ReservationsResponse
    {
        public string ReservationCode { get; set; }
        public string Client { get; set; }
        public string ClientDocument { get; set; }
        public string GroupCode { get; set; }
        public string GroupDescription { get; set; }
        public string AgencyCode { get; set; }
        public string AgencyName { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal ReservationValue { get; set; }
        public decimal SecurityDepositValue { get; set; }
        public ReservationStatus Status { get; set; }
        public int Days => ReturnDate.DayOfYear - PickupDate.DayOfYear;
    }
}
