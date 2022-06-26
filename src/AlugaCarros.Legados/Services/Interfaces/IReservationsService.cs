using AlugaCarros.Core.Dtos;
using AlugaCarros.Legados.Api.Models.Reservations;

namespace AlugaCarros.Legados.Api.Services.Interfaces;
public interface IReservationsService
{
    List<ReservationsResponse> GetReservations(string agencyCode);
    ReservationsResponse GetReservation(string reservationCode);
    ResultDto GenerateReservation(string clientDocument, string vehicleGroupCode, string agencyCode, DateTime pickupDate, DateTime returnDate);
    ResultDto CloseReservation(string reservationCode);
}
