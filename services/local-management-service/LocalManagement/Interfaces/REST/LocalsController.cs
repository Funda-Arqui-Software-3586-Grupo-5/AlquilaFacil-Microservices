using System.Net.Mime;
using LocalManagement.Domain.Model.Queries;
using LocalManagement.Domain.Services;
using LocalManagement.Interfaces.REST.Resources;
using LocalManagement.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocalManagement.Interfaces.REST;

/**
 * <summary>
 * Controller for managing locals.
 * </summary>
 * <remarks>
 * This controller provides endpoints for creating, retrieving, updating, and searching locals.
 * It also includes endpoints for getting all districts and user-specific locals.
 * </remarks>
 */
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class LocalsController(ILocalCommandService localCommandService, ILocalQueryService localQueryService)
:ControllerBase
{
    /**
     * POST: local/locals
     * <summary>
     *     Create local endpoint. It allows to create a new local.
     * </summary>
     * <remarks>
     * Create a new local with the provided details.&#xA;
     * It has the following properties: &#xA;
     * <b>localName</b>: Local's name &#xA;
     * <b>DescriptionMessage</b>: Local's description&#xA;
     * <b>localCategoryId</b>: Local's category ID&#xA;
     * <b>Capacity</b>: Local's capacity&#xA;
     * <b>District</b>: Local's district&#xA;
     * <b>Street</b>: Local's street&#xA;
     * <b>Country</b>: Local's country&#xA;
     * <b>City</b>: Local's city&#xA;
     * <b>Price</b>: Local's price&#xA;
     * <b>PhotoUrl</b>: Local's photo URL&#xA;
     * <b>UserId</b>: ID of the user creating the local&#xA;
     * <b>Features</b>: Features associated with the local&#xA;
     * <b>Sample request:</b>
     * {
     *   "localName": "Local Name",
     *   "DescriptionMessage": "Local Description",
     *   "localCategoryId": 2,
     *   "capacity": 100,
     *   "district": "Downtown",
     *   "street": "123 Main St",
     *   "country": "Country Name",
     *   "city": "City Name",
     *   "price": 50,
     *   "photoUrl": "http://example.com/photo.jpg",
     *   "userId": 1,
     *   "features": "WiFi, Parking"
     * }
     * </remarks>
     * <param name="resource">The resource containing local details.</param>
     * <returns>The created local resource</returns>
     * <response code="201">Returns the created local resource&#xA;
     * Success Example:
     * {
     *   "id": 1,
     *   "localName": "Local Name",
     *   "descriptionMessage": "Local Description",
     *   "categoryId": 2,
     *   "capacity": 100,
     *   "streetAddress": "Downtown, 123 Main St",
     *   "cityPlace": "Country Name, City Name",
     *   "nightPrice": 50,
     *   "photoUrl": "http://example.com/photo.jpg",
     *   "userId": 1,
     *   "features": "WiFi, Parking"
     * }
     * </response>
     * <response code="400">Error creating local&#xA;
     * Error Example:
     * {
     *  "message": "User does not exist"
     * }
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="403">If the user is not authorized to access this resource</response>
     * <response code="500">If there was an internal error</response>
     */
    [HttpPost]
    public async Task<IActionResult> CreateLocal(CreateLocalResource resource)
    {
        try {    
            var createLocalCommand = CreateLocalCommandFromResourceAssembler.ToCommandFromResources(resource);
            var local = await localCommandService.Handle(createLocalCommand);
            if (local is null) return BadRequest();
            var localResource = LocalResourceFromEntityAssembler.ToResourceFromEntity(local);
            return CreatedAtAction(nameof(GetLocalById), new { localId = localResource.Id }, localResource);
        }catch (Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
    }

    /**
     * GET: local/locals
     * <summary>
     *     Get all locals endpoint. It allows to retrieve all locals.
     * </summary>
     * <remarks>
     *  Get all locals.&#xA;
     * </remarks>
     * <returns>A list of local resources</returns>
     * <response code="200">Returns the list of local resources&#xA;
     * Success Example:
     * [
     *   {
     *     "id": 1,
     *     "localName": "Local Name",
     *     "descriptionMessage": "Local Description",
     *     "categoryId": 2,
     *     "capacity": 100,
     *     "streetAddress": "Downtown, 123 Main St",
     *     "cityPlace": "Country Name, City Name",
     *     "nightPrice": 50,
     *     "photoUrl": "http://example.com/photo.jpg",
     *     "userId": 1,
     *     "features": "WiFi, Parking"
     *   },
     *   {
     *      // Another local resource
     *   }
     * ]
     * </response>
     * <response code="400">Error obtaining locals&#xA;
     * Error Example:
     * {
     *   "message": "Error retrieving locals"
     * }
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="500">If there was an internal error</response>
     */
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> GetAllLocals()
    {
        try {
            var getAllLocalsQuery = new GetAllLocalsQuery();
            var locals = await localQueryService.Handle(getAllLocalsQuery);
            var localResources = locals.Select(LocalResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(localResources);
        }catch(Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    /**
     * GET: local/locals/{localId}
     * <summary>
     *     Get local by ID endpoint. It allows to retrieve a specific local by its ID.
     * </summary>
     * <remarks>
     * Get a local by its ID.&#xA;
     * It has the following properties: &#xA;
     * <b>localId</b>: Local's ID &#xA;
     * </remarks>
     * <param name="localId">The local ID</param>
     * <returns>The local resource</returns>
     * <response code="200">Returns the local resource&#xA;
     * Success Example:
     * {
     *   "id": 1,
     *   "localName": "Local Name",
     *   "descriptionMessage": "Local Description",
     *   "categoryId": 2,
     *   "capacity": 100,
     *   "streetAddress": "Downtown, 123 Main St",
     *   "cityPlace": "Country Name, City Name",
     *   "nightPrice": 50,
     *   "photoUrl": "http://example.com/photo.jpg",
     *   "userId": 1,
     *   "features": "WiFi, Parking"
     * }
     * </response>
     * <response code="400">Error obtaining local&#xA;
     * Error Example:
     * {
     *   "message": "Invalid local ID"
     * }
     * </response>
     * <response code="404">Local not found</response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="500">If there was an internal error</response>
     */
    [HttpGet("{localId:int}")]
    public async Task<IActionResult> GetLocalById(int localId)
    {
        try {
            var getLocalByIdQuery = new GetLocalByIdQuery(localId);
            var local = await localQueryService.Handle(getLocalByIdQuery);
            if (local == null) return NotFound();
            var localResource = LocalResourceFromEntityAssembler.ToResourceFromEntity(local);
            return Ok(localResource);
        }catch(Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    /**
     * PUT: local/locals/{localId}
     * <summary>
     *     Update local endpoint. It allows to update an existing local.
     * </summary>
     * <param name="localId">The local ID</param>
     * <remarks>
     * Update a local with the provided details.&#xA;
     * It has the following properties: &#xA;
     * <b>localName</b>: Local's name &#xA;
     * <b>DescriptionMessage</b>: Local's description&#xA;
     * <b>localCategoryId</b>: Local's category ID&#xA;
     * <b>Capacity</b>: Local's capacity&#xA;
     * <b>District</b>: Local's district&#xA;
     * <b>Street</b>: Local's street&#xA;
     * <b>Country</b>: Local's country&#xA;
     * <b>City</b>: Local's city&#xA;
     * <b>Price</b>: Local's price&#xA;
     * <b>PhotoUrl</b>: Local's photo URL&#xA;
     * <b>Features</b>: Features associated with the local&#xA;
     * <b>Sample request:</b>
     * {
     *    "localName": "Updated Local Name",
     *    "DescriptionMessage": "Updated Local Description",
     *    "localCategoryId": 2,
     *    "capacity": 100,
     *    "district": "Downtown",
     *    "street": "123 Main St",
     *    "country": "Country Name",
     *    "city": "City Name",
     *    "price": 50,
     *    "photoUrl": "http://example.com/photo.jpg",
     *    "userId": 1,
     *    "features": "WiFi, Parking"
     * }
     * </remarks>
     * <returns>The updated local resource</returns>
     * <response code="200">Returns the updated local resource&#xA;
     * Success Example:
     * {
     *   "id": 1,
     *   "localName": "Updated Local Name",
     *   "descriptionMessage": "Updated Local Description",
     *   "categoryId": 2,
     *   "capacity": 100,
     *   "streetAddress": "Downtown, 123 Main St",
     *   "cityPlace": "Country Name, City Name",
     *   "nightPrice": 50,
     *   "photoUrl": "http://example.com/photo.jpg",
     *   "userId": 1,
     *   "features": "WiFi, Parking"
     * }
     * </response>
     * <response code="400">Error updating local&#xA;
     * Error Example:
     * {
     *   "message": "Local not found"
     * }
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="500">If there was an internal error</response>
     * <response code="403">If the user is not authorized to access this resource</response>
     */
    [HttpPut("{localId:int}")]
    public async Task<IActionResult> UpdateLocal(int localId, UpdateLocalResource resource)
    {
        try {
            var updateLocalCommand = UpdateLocalCommandFromResourceAssembler.ToCommandFromResource(localId, resource);
            var local = await localCommandService.Handle(updateLocalCommand);
            if (local is null) return BadRequest();
            var localResource = LocalResourceFromEntityAssembler.ToResourceFromEntity(local);
            return Ok(localResource);
        }catch(Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    /**
    * GET: local/locals/search-by-category-id-capacity-range/{categoryId}/{minCapacity}/{maxCapacity}
    * <summary>
    *     Search locals by category and capacity range.
    * </summary>
     * <remarks>
     * Search for locals based on category ID and a range of capacities.&#xA;
     * It has the following properties: &#xA;
     * <b>categoryId</b>: Local category ID&#xA;
     * <b>minCapacity</b>: Minimum capacity&#xA;
     * <b>maxCapacity</b>: Maximum capacity
     * </remarks>
    * <param name="categoryId">Local category ID.</param>
    * <param name="minCapacity">Minimum capacity.</param>
    * <param name="maxCapacity">Maximum capacity.</param>
    * <returns>List of locals that meet the criteria.</returns>
    * <response code="200">Returns the list of filtered locals&#xA;
    * Success example:
    * [
    *   {
    *     "id": 1,
    *     "localName": "Event Hall",
    *     "descriptionMessage": "Spacious venue for events",
    *     "categoryId": 2,
    *     "capacity": 120,
    *     "streetAddress": "Downtown, Main Ave 123",
    *     "cityPlace": "Country, City",
    *     "nightPrice": 80,
    *     "photoUrl": "http://example.com/photo.jpg",
    *     "userId": 5,
    *     "features": "WiFi, Parking"
    *   }
    * ]
    * </response>
    * <response code="400">Error searching locals&#xA;
    * Error example:
    * {
    *   "message": "Invalid parameters"
    * }
    * </response>
     * <response code="401">If the user is not authenticated</response>
    * <response code="500">If an internal error occurs</response>
    */
    [HttpGet("search-by-category-id-capacity-range/{categoryId:int}/{minCapacity:int}/{maxCapacity:int}")]
    public async Task<IActionResult> SearchByCategoryIdAndCapacityRange(int categoryId, int minCapacity, int maxCapacity)
    {
        try {
            var searchByCategoryIdAndCapacityRangeQuery = new GetLocalsByCategoryIdAndCapacityRangeQuery(categoryId, minCapacity, maxCapacity);
            var locals = await localQueryService.Handle(searchByCategoryIdAndCapacityRangeQuery);
            var localResources = locals.Select(LocalResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(localResources);
        }catch(Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    /**
     * GET: local/districts
     * <summary>
     *     Get all districts endpoint. It allows to retrieve all districts.
     * </summary>
     * <remarks>
     * Get all districts.
     * </remarks>
     * <returns>A list of district resources</returns>
     * <response code="200">Returns the list of district resources&#xA;
     * Success Example:
     * [
     *   {
     *     "id": 1,
     *     "name": "Downtown"
     *   },
     *   {
     *     "id": 2,
     *     "name": "Uptown"
     *   }
     * ]
     * </response>
     * <response code="400">Error obtaining districts&#xA;
     * Error Example:
     * {
     *   "message": "Error retrieving districts"
     * }
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="500">If there was an internal error</response>
     */
    [HttpGet("get-all-districts")]
    public IActionResult GetAllDistricts()
    {
        try {
            var getAllLocalDistrictsQuery = new GetAllLocalDistrictsQuery();
            var districts = localQueryService.Handle(getAllLocalDistrictsQuery);
            return Ok(districts);
        }catch(Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    /**
     * GET: local/locals/get-user-locals/{userId}
     * <summary>
     *     Get user locals endpoint. It allows to retrieve all locals created by a specific user.
     * </summary>
     * <remarks>
     * Get all locals created by a specific user.&#xA;
     * It has the following property: &#xA;
     * <b>userId</b>: User's ID
     * </remarks>
     * <param name="userId">The user ID</param>
     * <returns>A list of local resources created by the user</returns>
     * <response code="200">Returns the list of local resources&#xA;
     * Success Example:
     * [
     *   {
     *     "id": 1,
     *     "localName": "User's Local",
     *     "descriptionMessage": "Description of user's local",
     *     "categoryId": 2,
     *     "capacity": 100,
     *     "streetAddress": "User's Street, 123",
     *     "cityPlace": "User's City, User's Country",
     *     "nightPrice": 50,
     *     "photoUrl": "http://example.com/user-local.jpg",
     *     "userId": 1,
     *     "features": "WiFi, Parking"
     *   }
     * ]
     * </response>
     * <response code="400">Error obtaining user's locals&#xA;
     * Error Example:
     * {
     *   "message": "Invalid user ID"
     * }
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="500">If there was an internal error</response>
     */
    [HttpGet("get-user-locals/{userId:int}")]
    public async Task<IActionResult> GetUserLocals(int userId)
    {
        try {
            var getUserLocalsQuery = new GetLocalsByUserIdQuery(userId);
            var locals = await localQueryService.Handle(getUserLocalsQuery);
            var localResources = locals.Select(LocalResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(localResources);
        }catch(Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
    }
    
}