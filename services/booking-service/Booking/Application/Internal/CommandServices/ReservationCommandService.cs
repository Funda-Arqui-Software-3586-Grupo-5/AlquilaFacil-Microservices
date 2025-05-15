using Booking.Application.External;
using Booking.Application.External.OutboundServices;
using Booking.Domain.Model.Aggregates;
using Booking.Domain.Model.Commands;
using Booking.Domain.Repositories;
using Booking.Domain.Services;
using Booking.Shared.Domain.Repositories;

namespace Booking.Application.Internal.CommandServices;

public class ReservationCommandService(
 IUserReservationExternalService userReservationExternalService,
 ILocalExternalService localExternalService, IReservationRepository reservationRepository,IUnitOfWork unitOfWork) : IReservationCommandService
{
 public async Task<Reservation> Handle(CreateReservationCommand reservation)
 {
     var userExists = await userReservationExternalService.UserExists(reservation.UserId);
     if (!userExists)
     {
         throw new Exception("User does not exist");
     }

     var localExists = await localExternalService.LocalReservationExists(reservation.LocalId);
     if (!localExists)
     {
         throw new Exception("Local does not exist");
     }
     if(reservation.StartDate > reservation.EndDate)
     {
         throw new Exception("Start date must be less than end date");
     }
     if (reservation.StartDate.Year < DateTime.Now.Year || reservation.StartDate.Month < DateTime.Now.Month || reservation.StartDate.Day < DateTime.Now.Day)
     {
         throw new Exception("Start date must be greater than current date");
     }
     if (reservation.EndDate.Year < DateTime.Now.Year || reservation.EndDate.Month < DateTime.Now.Month || reservation.EndDate.Day < DateTime.Now.Day)
     {
         throw new Exception("End date must be greater than current date");
     }

     if (await localExternalService.IsLocalOwner(reservation.UserId, reservation.LocalId))
     {
            throw new BadHttpRequestException("User is the owner of the local, he cannot make a reservation");
     }

     var reservationCreated = new Reservation(reservation);
     await reservationRepository.AddAsync(reservationCreated);
     await unitOfWork.CompleteAsync();
     return reservationCreated;

 }

 public async Task<Reservation> Handle(UpdateReservationDateCommand reservation)
 {
     if(reservation.StartDate > reservation.EndDate)
     {
         throw new Exception("Start date must be less than end date");
     }
     if (reservation.StartDate.Year < DateTime.Now.Year || reservation.StartDate.Month < DateTime.Now.Month || reservation.StartDate.Day < DateTime.Now.Day)
     {
            throw new Exception("Start date must be greater than current date");
     }
     if (reservation.EndDate < DateTime.Now)
     {
         throw new Exception("End date must be greater than current date");
     }

     var reservationToUpdate = await reservationRepository.FindByIdAsync(reservation.Id);
        if (reservationToUpdate == null)
        {
            throw new Exception("Reservation does not exist");
        }
        reservationToUpdate.UpdateDate(reservation);
        reservationRepository.Update(reservationToUpdate);
        await unitOfWork.CompleteAsync();
        return reservationToUpdate;
 }

 public async Task<Reservation> Handle(DeleteReservationCommand reservation)
 {
     var reservationToDelete = await reservationRepository.FindByIdAsync(reservation.Id);
     if (reservationToDelete == null)
     {
         throw new Exception("Reservation does not exist");
     }

     reservationRepository.Remove(reservationToDelete);
     await unitOfWork.CompleteAsync();
     return reservationToDelete;
 }
}