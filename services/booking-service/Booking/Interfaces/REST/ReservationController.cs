using System.Net.Mime;
using Booking.Application.External;
using Booking.Domain.Model.Queries;
using Booking.Domain.Services;
using Booking.Interfaces.REST.Resources;
using Booking.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ReservationController(IReservationCommandService reservationCommandService, IReservationQueryService reservationQueryService, ISubscriptionExternalService subscriptionExternalService) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> CreateReservationAsync([FromBody]CreateReservationResource resource)
    {
        try {
            var command = CreateReservationCommandFromResourceAssembler.ToCommandFromResource(resource);
            var result = await reservationCommandService.Handle(command);
            var reservationResource = ReservationResourceFromEntityAssembler.ToResourceFromEntity(result);
            return StatusCode(201, reservationResource);
        }catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateReservationAsync(int id, [FromBody]UpdateReservationResource resource)
    {
        try {
            var command = UpdateReservationDateCommandFromResourceAssembler.ToCommandFromResource(id, resource);
            var result = await reservationCommandService.Handle(command);
            var reservationResource = ReservationResourceFromEntityAssembler.ToResourceFromEntity(result);
            return Ok(reservationResource);
        }catch(Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReservationAsync(int id)
    {
        try {
            var resource = new DeleteReservationResource(id);
            var command = DeleteReservationCommandFromResourceAssembler.ToCommandFromResource(resource);
            await reservationCommandService.Handle(command);
            return StatusCode(200, "Reservation deleted");
        }catch(Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    

    [HttpGet("by-user-id/{userId:int}")]
    public async Task<IActionResult> GetReservationsByUserIdAsync(int userId)
    {
        try {
            var query = new GetReservationsByUserId(userId);
            var result = await reservationQueryService.GetReservationsByUserIdAsync(query);
            var reservationResource = result.Select(ReservationResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(reservationResource);
        }catch(Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("reservation-user-details/{userId:int}")]
    public async Task<IActionResult> GetReservationUserDetailsAsync(int userId)
    {
        try {
            var query = new GetReservationsByOwnerIdQuery(userId);
            var locals = new List<LocalReservationResource>();

            var reservations = await reservationQueryService.GetReservationsByOwnerIdAsync(query);
            if (reservations == null || !reservations.Any())
            {
                return NotFound(new { message = "No reservations found for this user." });
            }
            
            
            var subscriptions = await subscriptionExternalService.GetSubscriptionByUsersId(reservations.Select(r => r.UserId).Distinct().ToList());
            var subscriptionDict = subscriptions
                .GroupBy(s => s.UserId)
                .ToDictionary(g => g.Key, g => g.First());
            
            foreach (var reservation in reservations)
            {
                subscriptionDict.TryGetValue(reservation.UserId, out var subscription);
                var localReservationResource = new LocalReservationResource(
                    reservation.Id,
                    reservation.StartDate,
                    reservation.EndDate,
                    reservation.UserId,
                    reservation.LocalId,
                    subscription?.PlanId == 1
                );
                locals.Add(localReservationResource);
            }

            var reservationDetailsResource = new ReservationDetailsResource(locals);
            return Ok(reservationDetailsResource);
        }catch(Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }





    [HttpGet("by-start-date/{startDate}")]
    public async Task<IActionResult> GetReservationByStartDateAsync(DateTime startDate)
    {
        try {
            var query = new GetReservationByStartDate(startDate);
            var result = await reservationQueryService.GetReservationByStartDateAsync(query);
            var reservationResource = result.Select(ReservationResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(reservationResource);
        }catch(Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("by-end-date/{endDate}")]
    public async Task<IActionResult> GetReservationByEndDateAsync(DateTime endDate)
    {
        try {
            var query = new GetReservationByEndDate(endDate);
            var result = await reservationQueryService.GetReservationByEndDateAsync(query);
            var reservationResource = result.Select(ReservationResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(reservationResource);
        }catch(Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
