using AlugaCarros.Core.Dtos;
using AlugaCarros.Legados.Api.Data;
using AlugaCarros.Legados.Api.Models.Clients;
using AlugaCarros.Legados.Api.Models.Reservations;
using AlugaCarros.Legados.Api.Services.Interfaces;
using Bogus;
using Dapper;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace AlugaCarros.Legados.Api.Services;
public class ReservationsService : IReservationsService
{
    private readonly LegadosDbContext _dbContext;

    public ReservationsService(LegadosDbContext legadosDbContext)
    {
        this._dbContext = legadosDbContext;
    }

    public List<ReservationsResponse> GetReservations(string agencyCode)
    {
        var qry = @"
                            Select r.Code ReservationCode, r.Status Status, c.Name Client, c.Document ClientDocument, vg.GroupCode GroupCode, vg.GroupDescription GroupDescription , a.AgencyCode AgencyCode, a.AgencyName AgencyName, 
                            r.PickupDate PickupDate, r.ReturnDate ReturnDate, r.ReservationValue ReservationValue , r.SecurityDepositValue SecurityDepositValue
                            From Reservations r 
                            Inner join clients c on r.IdClient = c.Id 
                            Inner join agencies a on a.Id = r.IdAgency 
                            Inner join vehiclegroups vg on vg.Id = r.IdVehicleGroup 
                            Where a.AgencyCode = @agencyCode
                            ";

        using var connection = new MySqlConnection(_dbContext.Database.GetDbConnection().ConnectionString);

        var reservations = connection.Query<ReservationsResponse>(qry, new { agencyCode }).ToList();

        return reservations;
    }

    public ReservationsResponse GetReservation(string reservationCode)
    {
        var qry = @"
                            Select r.Code ReservationCode, r.Status Status, c.Name Client, c.Document ClientDocument, vg.GroupCode GroupCode, vg.GroupDescription GroupDescription , a.AgencyCode AgencyCode, a.AgencyName AgencyName, 
                            r.PickupDate PickupDate, r.ReturnDate ReturnDate, r.ReservationValue ReservationValue , r.SecurityDepositValue SecurityDepositValue
                            From Reservations r 
                            Inner join clients c on r.IdClient = c.Id 
                            Inner join agencies a on a.Id = r.IdAgency 
                            Inner join vehiclegroups vg on vg.Id = r.IdVehicleGroup 
                            Where r.Code = @reservationCode
                            ";

        using var connection = new MySqlConnection(_dbContext.Database.GetDbConnection().ConnectionString);

        var reservation = connection.Query<ReservationsResponse>(qry, new { reservationCode }).FirstOrDefault();

        return reservation;
    }

    public ResultDto GenerateReservation(string clientDocument, string vehicleGroupCode, string agencyCode, DateTime pickupDate, DateTime returnDate)
    {
        var agency = _dbContext.Agencies.FirstOrDefault(f => f.AgencyCode == agencyCode);

        if (agency == null) return ResultDto.Failed($"Agência {agencyCode} não encontrada");

        var client = _dbContext.Clients.FirstOrDefault(f => f.Document == clientDocument);

        if (client == null)
        {
            client = new Faker<Client>("pt_BR")
                .CustomInstantiator(f => new Client(f.Person.FullName, f.Person.Email,
                f.Phone.PhoneNumber("(##) #####-####"), clientDocument, ClientStatus.Active));

            _dbContext.Clients.Add(client);
        }

        var vehicleGroup = _dbContext.VehicleGroups.FirstOrDefault(f => f.GroupCode == vehicleGroupCode);

        if (vehicleGroup == null) return ResultDto.Failed("Grupo de véiculo não encontrado");

        if (returnDate < pickupDate) return ResultDto.Failed("Data de retorno deve ser inferior a data de retirada");

        var reservation = new Reservation(client.Id, vehicleGroup.Id,agency.Id, pickupDate, returnDate);

        _dbContext.Reservations.Add(reservation);

        _dbContext.SaveChanges();

        return ResultDto.Success($"Reservation Code: {reservation.Code}");
    }

    public ResultDto CloseReservation(string reservationCode)
    {
        var reservation = _dbContext.Reservations.FirstOrDefault(f => f.Code == reservationCode);
        if(reservation == null) return ResultDto.Failed("Reserva não encontrada!");

        reservation.CloseReservation();

        _dbContext.Reservations.Update(reservation);

        _dbContext.SaveChanges();

        return ResultDto.Success("Reserva fechada com Sucesso!");
    }
}
