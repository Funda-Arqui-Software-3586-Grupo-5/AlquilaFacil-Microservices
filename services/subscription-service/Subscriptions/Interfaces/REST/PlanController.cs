using Subscriptions.Domain.Model.Queries;
using Subscriptions.Domain.Services;
using Subscriptions.Interfaces.REST.Resources;
using Subscriptions.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace Subscriptions.Interfaces.REST;

/**
 * <summary>
 * Controller for managing subscription plans.
 * </summary>
 * <remarks>
 * This controller provides an endpoint to retrieve all available subscription plans.
 * It is used to get a list of all subscription plans in the system.
 * </remarks>
 */
[ApiController]
[Route("api/v1/[controller]")]
public class PlanController(IPlanQueryService planQueryService) : ControllerBase
{
    /**
     * GET: subscription/plans
     * <summary>
     *     Get all subscription plans endpoint. It allows to retrieve all subscription plans.
     * </summary>
     * <returns>A list of plan resources</returns>
     * <response code="200">Returns the list of plan resources&#xA;
     * Success Example:
     * [
     *   {
     *     "id": 1,
     *     "name": "Plan Básico",
     *     "price": 9,
     *     "service": "El plan premium te permitira acceder a funcionalidades adicionales en la aplicacion"
     *   },
     *   {
     *     "id": 2,
     *    "name": "Plan Premium",
     *    "price": 20,
     *    "service": "El plan premium te permitira acceder a funcionalidades adicionales en la aplicacion"
     *   }
     * ]
     * </response>
     * <response code="400">Error obtaining plans&#xA;
     * Error Example:
     * {
     *   "message": "Error retrieving subscription plans"
     * }
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="500">If there was an internal error</response>
     */
    [HttpGet]
    public async Task<IActionResult> GetAllPlans()
    {
        try {
            var getAllPlansQuery = new GetAllPlansQuery();
            var plans = await planQueryService.Handle(getAllPlansQuery);
            var resources = plans.Select(PlanResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(resources);
        }catch(Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

}