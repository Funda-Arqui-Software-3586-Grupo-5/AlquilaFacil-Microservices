using System.Net.Mime;
using Booking.Application.External;
using Booking.Domain.Model.Queries;
using Booking.Domain.Services;
using Booking.Interfaces.REST.Resources;
using Booking.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace Booking.Interfaces.REST;

/**
 * <summary>
 * Controller for managing reservations.
 * </summary>
 * <remarks>
 * This controller provides endpoints for creating, updating, deleting, and retrieving reservations.
 * It allows users to manage their reservations effectively.
 * </remarks>
 */
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ReservationController(IReservationCommandService reservationCommandService, IReservationQueryService reservationQueryService, ISubscriptionExternalService subscriptionExternalService) : ControllerBase
{

    /**
     * POST: booking/reservation
     * <summary>
     *     Create a reservation endpoint. It allows to create a reservation.
     * </summary>
     * <remarks>
     * This endpoint allows users to create a reservation for a local. The reservation includes details such as the local ID, user ID, start date, and end date.&#xA;
     * It has the following properties:&#xA;
     * <b>startDate</b>: The start date of the reservation&#xA;
     * <b>endDate</b>: The end date of the reservation&#xA;
     * <b>localId</b>: The ID of the local being reserved&#xA;
     * <b>userId</b>: The ID of the user making the reservation&#xA;
     * <b>Sample request:</b>
     * {
     *    "startDate": "2023-10-01T10:00:00Z",
     *    "endDate": "2023-10-01T12:00:00Z",
     *    "localId": 1,
     *    "userId": 1
     * }
     * </remarks>
     * <param name="resource">The resource containing the reservation details.</param>
     * <returns>The created reservation resource</returns>
     * <response code="201">Returns the created reservation resource&#xA;
     * Success Example:
     * {
     *    "id": 1,
     *    "startDate": "2023-10-01T10:00:00Z",
     *    "endDate": "2023-10-01T12:00:00Z",
     *    "localId": 1,
     *    "userId": 1
     * }
     * </response>
     * <response code="400">Error creating reservation&#xA;
     * Error Example:
     * {
     *    "message": "Start date must be greater than current date"
     * }
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="403">If the user is not authorized to access this resource</response>
     * <response code="500">If there was an internal error</response>
     */
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

    /**
     * PUT: booking/reservation/{id}
     * <summary>
     *     Update a reservation endpoint. It allows to update a reservation's date.
     * </summary>
     * <remarks>
     * This endpoint allows users to update the start and end dates of an existing reservation.&#xA;
     * It has the following properties:&#xA;
     * <b>startDate</b>: The new start date of the reservation&#xA;
     * <b>endDate</b>: The new end date of the reservation&#xA;
     * <b>userId</b>: The ID of the user making the reservation&#xA;
     * <b>localId</b>: The ID of the local being reserved&#xA;
     * <b>Sample request:</b>
     * {
     *    "startDate": "2023-10-01T10:00:00Z",
     *    "endDate": "2023-10-01T12:00:00Z",
     *    "localId": 1,
     *    "userId": 1
     * }
     * </remarks>
     * <param name="id">The ID of the reservation to update.</param>
     * <param name="resource">The resource containing the updated reservation details.</param>
     * <returns>The updated reservation resource</returns>
     * <response code="200">Returns the updated reservation resource&#xA;
     * Success Example:
     * {
     *    "id": 1,
     *    "startDate": "2023-10-01T10:00:00Z",
     *    "endDate": "2023-10-01T12:00:00Z",
     *    "localId": 1,
     *    "userId": 1
     * }
     * </response>
     * <response code="400">Error updating reservation&#xA;
     * Error Example:
     * {
     *   "message": "Start date must be less than end date"
     * }
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="403">If the user is not authorized to access this resource</response>
     * <response code="500">If there was an internal error</response>
     */
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

    /**
     * DELETE: booking/reservation/{id}
     * <summary>
     *     Delete a reservation endpoint. It allows to delete a reservation.
     * </summary>
     * <remarks>
     * This endpoint allows users to delete an existing reservation by its ID.&#xA;
     * It has the following properties:&#xA;
     * <b>id</b>: The ID of the reservation to delete
     * </remarks>
     * <param name="id">The ID of the reservation to delete.</param>
     * <returns>A success message</returns>
     * <response code="200">Returns a success message&#xA;
     * Success Example:
     * "Reservation deleted"
     * </response>
     * <response code="400">Error deleting reservation&#xA;
     * Error Example:
     * {
     *   "message": "Reservation does not exist"
     * }
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="403">If the user is not authorized to access this resource</response>
     * <response code="500">If there was an internal error</response>
     */
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
    
    /**
     * GET: booking/reservation/by-user-id/{id}
     * <summary>
     *     Get a reservation by ID endpoint. It allows to get a reservation by its ID.
     * </summary>
     * <remarks>
     * This endpoint allows users to retrieve a reservation by its user ID.&#xA;
     * It has the following properties:&#xA;
     * <b>userId</b>: The ID of the user whose reservations are to be retrieved
     * </remarks>
     * <param name="userId">The ID of the user whose reservations are to be retrieved.</param>
     * <returns>The reservation resource</returns>
     * <response code="200">Returns the reservation resource&#xA;
     * Success Example:
     * {
     *    "id": 1,
     *    "startDate": "2023-10-01T10:00:00Z",
     *    "endDate": "2023-10-01T12:00:00Z",
     *    "localId": 1,
     *    "userId": 1
     * }
     * </response>
     * <response code="400">Error retrieving reservation&#xA;
     * Error Example:
     * {
     *   "message": "Reservation not found"
     * }
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="500">If there was an internal error</response>
     */
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

    /**
     * GET: booking/reservation/reservation-user-details/{userId}
     * <summary>
     *     Get reservation user details endpoint. It allows to get reservation details for a specific user.
     * </summary>
     * <remarks>
     * This endpoint allows users to retrieve reservation details for a specific user, including subscription status.&#xA;
     * It has the following properties:&#xA;
     * <b>userId</b>: The ID of the user whose reservations are to be retrieved
     * </remarks>
     * <param name="userId">The ID of the user whose reservations are to be retrieved.</param>
     * <returns>A list of local reservation resources with user details</returns>
     * <response code="200">Returns the list of local reservation resources&#xA;
     * Success Example:
     * [
     *   {
     *     "id": 1,
     *     "startDate": "2023-10-01T10:00:00Z",
     *     "endDate": "2023-10-01T12:00:00Z",
     *     "userId": 1,
     *     "localId": 1,
     *     "isSubscribe": true
     *   }
     * ]
     * </response>
     * <response code="404">No reservations found for this user&#xA;
     * Error Example:
     * {
     *   "message": "No reservations found for this user."
     * }
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="500">If there was an internal error</response>
     */
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

    /**
     * GET: booking/reservation/by-start-date/{startDate}
     * <summary>
     *     Get reservations by start date endpoint. It allows to retrieve reservations that start on a specific date.
     * </summary>
     * <remarks>
     * This endpoint allows users to retrieve reservations that start on a specific date.&#xA;
     * It has the following properties:&#xA;
     * <b>startDate</b>: The start date to filter reservations
     * </remarks>
     * <param name="startDate">The start date to filter reservations.</param>
     * <returns>A list of reservation resources that start on the specified date</returns>
     * <response code="200">Returns the list of reservation resources&#xA;
     * Success Example:
     * [
     *   {
     *     "id": 1,
     *     "startDate": "2023-10-01T10:00:00Z",
     *     "endDate": "2023-10-01T12:00:00Z",
     *     "localId": 1,
     *     "userId": 1
     *   }
     * ]
     * </response>
     * <response code="400">Error retrieving reservations&#xA;
     * Error Example:
     * {
     *   "message": "Invalid start date"
     * }
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="500">If there was an internal error</response>
     */
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

    /**
     * GET: booking/reservation/by-end-date/{endDate}
     * <summary>
     *     Get reservations by end date endpoint. It allows to retrieve reservations that end on a specific date.
     * </summary>
     * <remarks>
     * This endpoint allows users to retrieve reservations that end on a specific date.&#xA;
     * It has the following properties:&#xA;
     * <b>endDate</b>: The end date to filter reservations
     * </remarks>
     * <param name="endDate">The end date to filter reservations.</param>
     * <returns>A list of reservation resources that end on the specified date</returns>
     * <response code="200">Returns the list of reservation resources&#xA;
     * Success Example:
     * [
     *   {
     *     "id": 1,
     *     "startDate": "2023-10-01T10:00:00Z",
     *     "endDate": "2023-10-01T12:00:00Z",
     *     "localId": 1,
     *     "userId": 1
     *   }
     * ]
     * </response>
     * <response code="400">Error retrieving reservations&#xA;
     * Error Example:
     * {
     *   "message": "Invalid end date"
     * }
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="500">If there was an internal error</response>
     */
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
