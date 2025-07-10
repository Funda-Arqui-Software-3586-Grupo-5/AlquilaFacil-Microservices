using Subscriptions.Domain.Model.Commands;
using Subscriptions.Domain.Model.Queries;
using Subscriptions.Domain.Services;
using Subscriptions.Interfaces.REST.Resources;
using Subscriptions.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Subscriptions.Interfaces.ACL;

namespace Subscriptions.Interfaces.REST;

/**
 * <summary>
 * Controller for managing subscriptions.
 * </summary>
 * <remarks>
 * This controller provides endpoints for creating, retrieving, and updating subscriptions.
 * It allows users to manage their subscriptions to various services.
 * </remarks>
 */
[ApiController]
[Route("api/v1/[controller]")]
public class SubscriptionsController(
    ISubscriptionContextFacade subscriptionContextFacade,
    ISubscriptionCommandService subscriptionCommandService,
    ISubscriptionQueryServices subscriptionQueryServices)
    : ControllerBase
{
    /**
     * POST: subscription/subscriptions
     * <summary>
     *     Create a subscription endpoint. It allows to create a new subscription.
     * </summary>
     * <remarks>
     * This endpoint allows users to create a new subscription. The request body should contain the details of the subscription.&#xA;
     * The subscription includes details such as the user ID, service ID, and subscription status.&#xA;
     * It has the following properties:&#xA;
     * <b>userId</b>: The ID of the user subscribing&#xA;
     * <b>planId</b>: The ID of the plan being subscribed to&#xA;
     * Sample request:
     * {
     *    "userId": 1,
     *    "planId": 1
     * }
     * </remarks>
     * <param name="createSubscriptionResource">The resource containing the subscription details.</param>
     * <returns>The created subscription resource</returns>
     * <response code="201">Returns the created subscription resource&#xA;
     * Success Example:
     * {
     *     "id": 1,
     *     "userId": 1,
     *     "planId": 1,
     *     "subscriptionStatusId": 1
     * }
     * </response>
     * <response code="400">Error creating subscription&#xA;
     * Error Example:
     * {
     *   "message": "Uuser ID or plan ID not found"
     * }
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="403">If the user is not authorized to access this resource</response>
     * <response code="500">If there was an internal error</response>
     */
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
    
    /**
     * GET: subscription/subscriptions
     * <summary>
     *     Get all subscriptions endpoint. It allows to get all subscriptions.
     * </summary>
     * <remarks>
     * This endpoint retrieves a list of all subscriptions.
     * </remarks>
     * <returns>A list of all subscriptions</returns>
     * <response code="200">Returns a list of all subscriptions&#xA;
     * Success Example:
     * [
     *   {
     *     "id": 1,
     *     "userId": 1,
     *     "planId": 1,
     *     "subscriptionStatusId": 1
     *   },
     *   {
     *     "id": 2,
     *     "userId": 2,
     *     "planId": 2,
     *     "subscriptionStatusId": 2
     *   }
     * ]
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="403">If the user is not authorized to access this resource</response>
     * <response code="500">If there was an internal error</response>
     */
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

    /**
     * GET: subscription/subscriptions/{userId:int}/subscribed
     * <summary>
     *     Check if a user is subscribed endpoint. It allows to check if a user is subscribed to any service.
     * </summary>
     * <remarks>
     * This endpoint checks if a user is subscribed to any service.&#xA;
     * It has the following properties:&#xA;
     * <b>userId</b>: The ID of the user to check subscription status for
     * </remarks>
     * <param name="userId">The ID of the user</param>
     * <returns>True if the user is subscribed, false otherwise</returns>
     * <response code="200">Returns true if the user is subscribed&#xA;
     * Success Example:
     * true
     * </response>
     * <response code="400">Error checking subscription status&#xA;
     * Error Example:
     * {
     *   "message": "User ID not found"
     * }
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="500">If there was an internal error</response>
     */
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
    
    /**
     * GET: subscription/subscriptions/subscriptions/by-users
     * <summary>
     *     Get subscriptions by users endpoint. It allows to get all subscriptions for a list of users.
     * </summary>
     * <remarks>
     * This endpoint retrieves all subscriptions for a list of users.
     * </remarks>
     * <returns>A list of subscription resources for the specified users</returns>
     * <param name="usersId">A list of user IDs to retrieve subscriptions for</param>
     * <response code="200">Returns the list of subscription resources&#xA;
     * Success Example:
     * [
     *   {
     *     "id": 1,
     *     "userId": 1,
     *     "planId": 1,
     *     "subscriptionStatusId": 1
     *   },
     *   {
     *     "id": 2,
     *     "userId": 2,
     *     "planId": 2,
     *     "subscriptionStatusId": 2
     *   }
     * ]
     * </response>
     * <response code="400">Error obtaining subscriptions&#xA;
     * Error Example:
     * {
     *   "message": "Invalid user IDs"
     * }
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="403">If the user is not authorized to access this resource</response>
     * <response code="500">If there was an internal error</response>
     */
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
    
    /**
     * GET: subscription/subscriptions/{subscriptionId}
     * <summary>
     *     Get subscription by ID endpoint. It allows to get a subscription by its ID.
     * </summary>
     * <remarks>
     * This endpoint retrieves a subscription by its ID.&#xA;
     * It has the following properties:&#xA;
     * <b>subscriptionId</b>: The ID of the subscription to retrieve
     * </remarks>
     * <param name="subscriptionId">The ID of the subscription</param>
     * <returns>The subscription resource</returns>
     * <response code="200">Returns the subscription resource&#xA;
     * Success Example:
     * {
     *   "id": 1,
     *   "userId": 1,
     *   "planId": 1,
     *   "subscriptionStatusId": 1
     * }
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="403">If the user is not authorized to access this resource</response>
     * <response code="404">Error obtaining subscription&#xA;
     * Error Example:
     * {
     *   "message": "Subscription not found"
     * }
     * </response>
     * <response code="500">If there was an internal error</response>
     */
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

    /**
     * PUT: subscription/subscriptions/{subscriptionId}/{subscriptionStatusId}
     * <summary>
     *     Update subscription status endpoint. It allows to update the status of a subscription.
     * </summary>
     * <remarks>
     * This endpoint updates the status of a subscription.&#xA;
     * It has the following properties:&#xA;
     * <b>subscriptionId</b>: The ID of the subscription to update&#xA;
     * <b>subscriptionStatusId</b>: The new status ID for the subscription
     * </remarks>
     * <param name="subscriptionId">The ID of the subscription</param>
     * <param name="subscriptionStatusId">The new status ID for the subscription</param>
     * <returns>The updated subscription resource</returns>
     * <response code="200">Returns the updated subscription resource&#xA;
     * Success Example:
     * {
     *   "id": 1,
     *   "userId": 1,
     *   "planId": 1,
     *   "subscriptionStatusId": 2
     * }
     * </response>
     * <response code="400">Error updating subscription status&#xA;
     * Error Example:
     * {
     *   "message": "Subscription or Status not found"
     * }
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="403">If the user is not authorized to access this resource</response>
     * <response code="500">If there was an internal error</response>
     */
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