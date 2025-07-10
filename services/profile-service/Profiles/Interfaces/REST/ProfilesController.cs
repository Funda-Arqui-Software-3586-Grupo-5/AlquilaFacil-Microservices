using System.Net.Mime;
using AlquilaFacilPlatform.Profiles.Domain.Model.Queries;
using AlquilaFacilPlatform.Profiles.Domain.Services;
using AlquilaFacilPlatform.Profiles.Interfaces.REST.Resources;
using AlquilaFacilPlatform.Profiles.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Profiles.Interfaces.REST.Transform;

namespace AlquilaFacilPlatform.Profiles.Interfaces.REST;

/**
 * <summary>
 * Controller for managing user profiles.
 * </summary>
 * <remarks>
 * This controller provides endpoints for creating and retrieving user profiles.
 * It allows users to create a profile, retrieve all profiles, and get a profile by ID or user ID.
 * </remarks>
 */
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ProfilesController(
    IProfileCommandService profileCommandService, 
    IProfileQueryService profileQueryService)
    : ControllerBase
{
    
    /**
     * POST: profile/profiles
     * <summary>
     *     Create a new user profile.
     * </summary>
     * <remarks>
     * This endpoint allows you to create a new user profile by providing the necessary details in the request body.&#xA;
     * It has the following properties:&#xA;
     * <b>Name</b>: The name of the user.&#xA;
     * <b>fatherName</b>: The father's name of the user.&#xA;
     * <b>motherName</b>: The mother's name of the user.&#xA;
     * <b>Phone</b>: The phone number of the user.&#xA;
     * <b>documentNumber</b>: The national identification number of the user.&#xA;
     * <b>dateOfBirth</b>: The date of birth of the user.&#xA;
     * <b>userId</b>: The ID of the user to whom the profile belongs.&#xA;
     * <b>photoUrl</b>: The URL of the user's profile photo.&#xA;
     * <b>Sample request:</b>
     * {
     *    "name": "John",
     *    "fatherName": "Doe",
     *    "motherName": "Smith",
     *    "phone": "1234567890",
     *    "documentNumber": "123456789",
     *    "dateOfBirth": "1990-01-01",
     *    "userId": 1,
     *    "photoUrl": "http://example.com/photo.jpg"
     * }
     * </remarks>
     * <param name="createProfileResource">The resource containing the profile details.</param>
     * <returns>The created profile resource.</returns>
     * <response code="201">Returns the created profile resource.&#xA;
     * Success Example:
     * {
     *    "id": 1,
     *    "fullName": "John Doe Smith",
     *    "phone": "1234567890",
     *    "documentNumber": "123456789",
     *    "dateOfBirth": "1990-01-01",
     *    "userId": 1,
     *    "photoUrl": "http://example.com/photo.jpg"
     * }
     * </response>
     * <response code="400">If the profile could not be created due to invalid input.&#xA;
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
    public async Task<IActionResult> CreateProfile([FromBody] CreateProfileResource createProfileResource)
    {
        var createProfileCommand = CreateProfileCommandFromResourceAssembler.ToCommandFromResource(createProfileResource);
        var profile = await profileCommandService.Handle(createProfileCommand);
        if (profile is null) return BadRequest();
        var resource = ProfileResourceFromEntityAssembler.ToResourceFromEntity(profile);
        return CreatedAtAction(nameof(GetProfileById), new {profileId = resource.Id}, resource);
    }
    
    /**
     * GET: profile/profiles
     * <summary>
     *     Retrieve all user profiles.
     * </summary>
     * <remarks>
     * This endpoint allows you to retrieve a list of all user profiles.
     * </remarks>
     * <returns>A list of profile resources.</returns>
     * <response code="200">Returns a list of profile resources.&#xA;
     * Success Example:
     * [
     *    {
     *        "id": 1,
     *        "fullName": "John Doe Smith",
     *        "phone": "1234567890",
     *        "documentNumber": "123456789",
     *        "dateOfBirth": "1990-01-01",
     *        "userId": 1,
     *        "photoUrl": "http://example.com/photo.jpg"
     *    },
     *    {
     *        "id": 2,
     *        "fullName": "Jane Doe Smith",
     *        "phone": "0987654321",
     *        "documentNumber": "987654321",
     *        "dateOfBirth": "1992-02-02",
     *        "userId": 2,
     *        "photoUrl": "http://example.com/photo2.jpg"
     *    }
     * ]
     * </response>
     * <response code="400">If there was an error retrieving the profiles.</response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="500">If there was an internal error</response>
     */
    [HttpGet]
    public async Task<IActionResult> GetAllProfiles()
    {
        var getAllProfilesQuery = new GetAllProfilesQuery();
        var profiles = await profileQueryService.Handle(getAllProfilesQuery);
        var resources = profiles.Select(ProfileResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
    
    /**
     * GET: profile/profiles/{profileId}
     * <summary>
     *     Retrieve a user profile by ID.
     * </summary>
     * <remarks>
     * This endpoint allows you to retrieve a user profile by its ID&#xA;
     * It has the following properties:&#xA;
     * <b>profileId</b>: The ID of the profile you want to retrieve.
     * </remarks>
     * <param name="profileId">The ID of the profile to retrieve.</param>
     * <returns>The profile resource with the specified ID.</returns>
     * <response code="200">Returns the profile resource.&#xA;
     * Success Example:
     * {
     *    "id": 1,
     *    "fullName": "John Doe Smith",
     *    "phone": "1234567890",
     *    "documentNumber": "123456789",
     *    "dateOfBirth": "1990-01-01",
     *    "userId": 1,
     *    "photoUrl": "http://example.com/photo.jpg"
     * }
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="404">If the profile with the specified ID does not exist.</response>
     * <response code="500">If there was an internal error</response>
     */
    [HttpGet("{profileId}")]
    public async Task<IActionResult> GetProfileById(int profileId)
    {
        var profile = await profileQueryService.Handle(new GetProfileByIdQuery(profileId));
        if (profile == null) return NotFound();
        var profileResource = ProfileResourceFromEntityAssembler.ToResourceFromEntity(profile);
        return Ok(profileResource);
    }
    
    /**
     * GET: profile/profiles/user/{userId}
     * <summary>
     *     Retrieve a user profile by user ID.
     * </summary>
     * <remarks>
     * This endpoint allows you to retrieve a user profile by the user's ID&#xA;
     * It has the following properties:&#xA;
     * <b>userId</b>: The ID of the user whose profile you want to retrieve.
     * </remarks>
     * <param name="userId">The ID of the user whose profile to retrieve.</param>
     * <returns>The profile resource associated with the specified user ID.</returns>
     * <response code="200">Returns the profile resource.&#xA;
     * Success Example:
     * {
     *    "id": 1,
     *    "fullName": "John Doe Smith",
     *    "phone": "1234567890",
     *    "documentNumber": "123456789",
     *    "dateOfBirth": "1990-01-01",
     *    "userId": 1,
     *    "photoUrl": "http://example.com/photo.jpg"
     * }
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="404">If the profile for the specified user ID does not exist.</response>
     * <response code="500">If there was an internal error</response>
     */
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetProfileByUserId(int userId)
    {
        var profile = await profileQueryService.Handle(new GetProfileByUserIdQuery(userId));
        if (profile == null) return NotFound();
        var profileResource = ProfileResourceFromEntityAssembler.ToResourceFromEntity(profile);
        return Ok(profileResource);
    }
    
    /**
     * GET: profile/profiles/is-user-subscribed/{userId}
     * <summary>
     *     Check if a user is subscribed.
     * </summary>
     * <remarks>
     * This endpoint allows you to check if a user is subscribed by their user ID.&#xA;
     * It has the following properties:&#xA;
     * <b>userId</b>: The ID of the user to check.
     * </remarks>
     * <param name="userId">The ID of the user to check.</param>
     * <returns>A boolean indicating whether the user is subscribed.</returns>
     * <response code="200">Returns true if the user is subscribed, false otherwise.&#xA;
     * Success Example:
     * true
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="404">If the user with the specified ID does not subscribed.</response>
     * <response code="500">If there was an internal error</response>
     */
    [HttpGet("is-user-subscribed/{userId}")]
    public async Task<IActionResult> IsUserSubscribed(int userId)
    {
        var query = new IsUserSubscribeQuery(userId);
        var user = await profileQueryService.Handle(query);
        return Ok(user);
    }

    /**
     * PUT: profile/profiles/{id}
     * <summary>
     *     Update a user profile by ID.
     * </summary>
     * <remarks>
     * This endpoint allows you to update a user profile by its ID.&#xA;
     * You need to provide the updated profile details in the request body.&#xA;
     * It has the following properties:&#xA;
     * <b>Name</b>: The name of the user.&#xA;
     * <b>fatherName</b>: The father's name of the user.&#xA;
     * <b>motherName</b>: The mother's name of the user.&#xA;
     * <b>Phone</b>: The phone number of the user.&#xA;
     * <b>documentNumber</b>: The national identification number of the user.&#xA;
     * <b>dateOfBirth</b>: The date of birth of the user.&#xA;
     * <b>userId</b>: The ID of the user to whom the profile belongs.&#xA;
     * <b>photoUrl</b>: The URL of the user's profile photo.&#xA;
     * <b>Sample request:</b>
     * {
     *    "name": "John",
     *    "fatherName": "Doe",
     *    "motherName": "Smith",
     *    "phone": "9994567890",
     *    "documentNumber": "123456789",
     *    "dateOfBirth": "1990-01-01",
     *    "userId": 1,
     *    "photoUrl": "http://example.com/photo.jpg"
     * }
     * </remarks>
     * <param name="id">The ID of the profile to update.</param>
     * <param name="updateProfileResource">The resource containing the updated profile details.</param>
     * <returns>The updated profile resource.</returns>
     * <response code="200">Returns the updated profile resource.&#xA;
     * Success Example:
     * {
     *    "id": 1,
     *    "fullName": "John Doe Smith Updated",
     *    "phone": "1234567890",
     *    "documentNumber": "123456789",
     *    "dateOfBirth": "1990-01-01",
     *    "userId": 1,
     *    "photoUrl": "http://example.com/photo.jpg"
     * }
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="403">If the user is not authorized to access this resource</response>
     * <response code="404">If the profile with the specified ID does not exist.</response>
     * <response code="500">If there was an internal error</response>
     */
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateFarm(int id,[FromBody] UpdateProfileResource updateProfileResource)
    {
        var updateProfileCommand = UpdateProfileCommandFromResourceAssembler.ToCommandFromResource(updateProfileResource,id);
        var result = await profileCommandService.Handle(updateProfileCommand);
        return Ok(ProfileResourceFromEntityAssembler.ToResourceFromEntity(result));
    }
}