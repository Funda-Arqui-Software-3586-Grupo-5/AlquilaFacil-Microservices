using System.Net.Mime;
using LocalManagement.Domain.Model.Queries;
using LocalManagement.Domain.Services;
using LocalManagement.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace LocalManagement.Interfaces.REST;

/**
 * <summary>
 * Controller for managing local categories.
 * </summary>
 * <remarks>
 * This controller provides an endpoint to retrieve all local categories.
 * It is used to get a list of all available local categories in the system.
 * </remarks>
 */
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class LocalCategoriesController(ILocalCategoryQueryService localCategoryQueryService)
    : ControllerBase
{
    /**
     * GET: local/local-categories
     * <summary>
     *     Get all local categories endpoint. It allows to retrieve all local categories.
     * </summary>
     * <remarks>
     * This endpoint retrieves all local categories available in the system.&#xA;
     * It returns a list of local categories, each containing an ID, name, and photo URL.
     * </remarks>
     * <returns>A list of local category resources</returns>
     * <response code="200">Returns the list of local category resources&#xA;
     * Success Example:
     * [
     *   {
     *     "id": 1,
     *     "name": "Restaurants",
     *     "photoUrl": "http://example.com/restaurant.jpg"
     *   },
     *   {
     *     "id": 2,
     *     "name": "Cafes",
     *    "photoUrl": "http://example.com/cafe.jpg"
     *   }
     * ]
     * </response>
     * <response code="400">Error obtaining local categories&#xA;
     * Error Example:
     * {
     *   "message": "Error retrieving local categories"
     * }
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="500">If there was an internal error</response>
     */
    [HttpGet]
    public async Task<IActionResult> GetAllLocalCategories()
    {
        try {
            var getAllLocalCategoriesQuery = new GetAllLocalCategoriesQuery();
            var localCategories = await localCategoryQueryService.Handle(getAllLocalCategoriesQuery);
            var localCategoryResources = localCategories.Select(LocalCategoryResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(localCategoryResources);
        }catch(Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
    }
}
