using Subscriptions.Domain.Model.Queries;
using Subscriptions.Domain.Services;
using Subscriptions.Interfaces.REST.Resources;
using Subscriptions.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace Subscriptions.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
public class PlanController(IPlanQueryService planQueryService) : ControllerBase
{
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