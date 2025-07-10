using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Notification.Domain.Models.Commands;
using Notification.Domain.Models.Queries;
using Notification.Domain.Services;
using Notification.Interfaces.REST.Resources;
using Notification.Interfaces.REST.Transforms;

namespace Notification.Interfaces.REST;

/**
 * <summary>
 * Controller for managing notifications.
 * </summary>
 * <remarks>
 * This controller provides endpoints for creating, retrieving, and deleting notifications.
 * It allows users to manage their notifications effectively.
 * </remarks>
 */
[Produces(MediaTypeNames.Application.Json)]
[ApiController]
[Route("api/v1/[controller]")]
public class NotificationController(INotificationCommandService notificationCommandService, INotificationQueryService notificationQueryService) : ControllerBase
{

    /**
     * POST: notification/notification
     * <summary>
     *     Save a notification endpoint. It allows to create a new notification.
     * </summary>
     * <remarks>
     * This endpoint allows users to create a new notification. The request body should contain the details of the notification.&#xA;
     * The notification includes details such as user ID, message, and timestamp.&#xA;
     * It has the following properties:&#xA;
     * <b>userId</b>: The ID of the user receiving the notification&#xA;
     * <b>title</b>: The title of the notification&#xA;
     * <b>description</b>: A detailed description of the notification&#xA;
     * Sample request:
     * {
     *    "userId": 1,
     *    "title": "New Message",
     *    "description": "You have a new message from John Doe."
     * }
     * </remarks>
     * <param name="createNotificationResource">The resource containing the notification details.</param>
     * <returns>The created notification resource</returns>
     * <response code="201">Returns the created notification resource&#xA;
     * Success Example:
     * {
     *   "id": 1,
     *   "userId": 1,
     *   "title": "New Message",
     *   "content": "You have a new message from John Doe.",
     * }
     * </response>
     * <response code="400">Error creating notification&#xA;
     * Error Example:
     * {
     *   "message": "Invalid input data"
     * }
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="403">If the user is not authorized to access this resource</response>
     * <response code="500">If there was an internal error</response>
     */
    [HttpPost]
    public async Task<IActionResult> SaveNotification([FromBody] CreateNotificationResource createNotificationResource)
    {
        try {
            var command = CreateNotificationCommandFromResourceAssembler.ToCommandFromResource(createNotificationResource);
            var notification = await notificationCommandService.Handle(command);
            var notificationResource = NotificationResourceFromEntityAssembler.ToResourceFromEntity(notification);
            return StatusCode(201, notificationResource);
        }catch(Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    /**
     * GET: notification/notification/{userId}
     * <summary>
     *     Get notifications by user ID endpoint. It allows to retrieve notifications for a specific user.
     * </summary>
     * <remarks>
     * This endpoint retrieves all notifications for a specific user based on their user ID.&#xA;
     * It has the following properties:&#xA;
     * <b>userId</b>: The ID of the user whose notifications are being retrieved&#xA;
     * </remarks>
     * <param name="userId">The ID of the user whose notifications are being retrieved.</param>
     * <returns>A list of notification resources</returns>
     * <response code="200">Returns the list of notification resources&#xA;
     * Success Example:
     * [
     *   {
     *      "id": 1,
     *      "userId": 1,
     *      "title": "New Message",
     *      "content": "You have a new message from John Doe."
     *   },
     *   {
     *      "id": 2,
     *      "userId": 1,
     *      "title": "Reminder",
     *      "content": "Don't forget your appointment tomorrow."
     *   }
     * ]
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="403">If the user is not authorized to access this resource</response>
     * <response code="500">If there was an internal error</response>
     */
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetNotificationsByUserId(int userId)
    {
        try {
            var query = new GetNotificationsByUserIdQuery(userId);
            var notifications = await notificationQueryService.Handle(query);
            var notificationResources = notifications.Select(NotificationResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(notificationResources);
        }catch(Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    /**
     * DELETE: notification/notification/{notificationId}
     * <summary>
     *     Delete a notification endpoint. It allows to delete a specific notification by its ID.
     * </summary>
     * <remarks>
     * This endpoint allows users to delete a specific notification by its ID.&#xA;
     * It has the following properties:&#xA;
     * <b>notificationId</b>: The ID of the notification to be deleted&#xA;
     * </remarks>
     * <param name="notificationId">The ID of the notification to be deleted.</param>
     * <returns>The deleted notification resource</returns>
     * <response code="200">Returns the deleted notification resource&#xA;
     * Success Example:
     * {
     *   "id": 1,
     *   "userId": 1,
     *   "title": "New Message",
     *   "content": "You have a new message from John Doe."
     * }
     * </response>
     * <response code="400">Error deleting notification&#xA;
     * Error Example:
     * {
     *   "message": "Notification not found"
     * }
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="403">If the user is not authorized to access this resource</response>
     * <response code="500">If there was an internal error</response>
     */
    [HttpDelete("{notificationId}")]
    public async Task<IActionResult> DeleteNotification(int notificationId)
    {
        try {
            var command = new DeleteNotificationCommand(notificationId);
            var notification = await notificationCommandService.Handle(command);
            return StatusCode(200, notification);
        }catch(Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
    }
}