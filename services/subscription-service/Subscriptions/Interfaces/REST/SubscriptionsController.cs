using Subscriptions.Domain.Model.Commands;
using Subscriptions.Domain.Model.Queries;
using Subscriptions.Domain.Services;
using Subscriptions.Interfaces.REST.Resources;
using Subscriptions.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Subscriptions.Interfaces.ACL;

namespace Subscriptions.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
public class SubscriptionsController(
    ISubscriptionContextFacade subscriptionContextFacade,
    ISubscriptionCommandService subscriptionCommandService,
    ISubscriptionQueryServices subscriptionQueryServices)
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateSubscription(
        [FromBody] CreateSubscriptionResource createSubscriptionResource)
    {
        try {    
            var createSubscriptionCommand =
                CreateSubscriptionCommandFromResourceAssembler.ToCommandFromResource(createSubscriptionResource);
            var subscription = await subscriptionCommandService.Handle(createSubscriptionCommand);
            if (subscription is null) return BadRequest();
            var resource = SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription);
            
            return CreatedAtAction(nameof(GetSubscriptionById), new { subscriptionId = resource.Id }, resource);
        }catch(Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllTutorials()
    {
        try {
            var getAllSubscriptionsQuery = new GetAllSubscriptionsQuery();
            var subscriptions = await subscriptionQueryServices.Handle(getAllSubscriptionsQuery);
            var resources = subscriptions.Select(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(resources);
        }catch(Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{userId:int}/subscribed")]

    public async Task<IActionResult> IsUserSubscribed(int userId)
    {
        try {
            var response = await subscriptionContextFacade.IsUserSubscribed(userId);
            return Ok(response);
        }catch(Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpGet("subscriptions/by-users")]
    public async Task<IActionResult> GetSubscriptionsByUsers([FromQuery] List<int> usersId)
    {
        try {
            var subscriptions = await subscriptionContextFacade.GetSubscriptionByUsersId(usersId);
            var resources = subscriptions.Select(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(resources);
        }catch(Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    
    
    [HttpGet("{subscriptionId}")]
    public async Task<IActionResult> GetSubscriptionById([FromRoute] int subscriptionId)
    {
        try {
            var subscription = await subscriptionQueryServices.Handle(new GetSubscriptionByIdQuery(subscriptionId));
            if (subscription == null) return NotFound();
            var resource = SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription);
            return Ok(resource);
        }catch(Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{subscriptionId}/{subscriptionStatusId}")]
    public async Task<IActionResult> UpdateSubscriptionStatus(int subscriptionId, int subscriptionStatusId)
    {
        try {
            var updateSubscriptionStatusCommand = new UpdateSubscriptionStatusCommand(subscriptionId, subscriptionStatusId);
            var subscription = await subscriptionCommandService.Handle(updateSubscriptionStatusCommand);
            if (subscription == null) return NotFound();
            var resource = SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription);
            return Ok(resource);
        }catch(Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
   
}