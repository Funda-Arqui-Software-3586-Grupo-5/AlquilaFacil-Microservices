using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Notification.Domain.Models.Commands;
using Notification.Domain.Models.Queries;
using Notification.Domain.Services;
using Notification.Interfaces.REST.Resources;
using Notification.Interfaces.REST.Transforms;

namespace Notification.Interfaces.REST;

[Produces(MediaTypeNames.Application.Json)]
[ApiController]
[Route("api/v1/[controller]")]
public class NotificationController(INotificationCommandService notificationCommandService, INotificationQueryService notificationQueryService) : ControllerBase
{

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