using System.Net.Mime;
using IAM.Application.Internal.CommandServices;
using IAM.Domain.Model.Aggregates;
using IAM.Domain.Model.Commands;
using IAM.Domain.Model.Queries;
using IAM.Domain.Services;
using IAM.Infrastructure.Pipeline.Middleware.Attributes;
using IAM.Interfaces.ACL;
using IAM.Interfaces.REST.Resources;
using IAM.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace IAM.Interfaces.REST;

/**
 * <summary>
 *     The users controller
 * </summary>
 * <remarks>
 *     This class is used to handle user requests
 * </remarks>
 */

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class UsersController(
    IIamContextFacade iamContextFacade,
    IUserQueryService userQueryService, IUserCommandService userCommandService
    ) : ControllerBase
{
    
    
    /**
     * GET: iam/users/{userId}
     * <summary>
     *     Get user by id endpoint. It allows to get a user by id
     * </summary>
     * <remarks>
     * Get a user based on the user id provided.&#xA;
     * It has the following property: &#xA;
     * <b>Id</b>: User's id
     * </remarks>
     * <param name="userId">The user id</param>
     * <returns>The user resource</returns>
     * <response code="200">Returns the user resource&#xA;
     * Success Example:
     * {
     *   "id": 1,
     *   "username": "Pruebita"
     * }
     * </response>
     * <response code="400">Error obtaining user&#xA;
     * Error Example:
     * {
     *   "message": "Object reference not set to an instance of an object."
     * }
     * </response>
     * <response code="500">If there was an internal error</response>
     */
    [HttpGet("{userId:int}")]
    public async Task<IActionResult> GetUserById(int userId)
    {
        try {
            var getUserByIdQuery = new GetUserByIdQuery(userId);
            var user = await userQueryService.Handle(getUserByIdQuery);
            var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(user!);
            return Ok(userResource);
        }catch (Exception e) {
            return BadRequest(new { message = e.Message });
        }
    }
    
    
    /**
     * GET: iam/users
     * <summary>
     *     Get all users endpoint. It allows to get all users
     * </summary>
     * <remarks>
     * Get all users.
     * </remarks>
     * <returns>The user resources</returns>
     * <response code="200">Returns the user resources&#xA;
     * Success Example:
     * [
     *   {
     *     "id": 1,
     *     "username": "Pruebita"
     *   },
     *   {
     *     "id": 2,
     *     "username": "Prueba1"
     *   }
     * ]
     * </response>
     * <response code="400">Error obtaining users</response>
     * <response code="500">If there was an internal error</response>
     */
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        try {
            var getAllUsersQuery = new GetAllUsersQuery();
            var users = await userQueryService.Handle(getAllUsersQuery);
            var userResources = users.Select(UserResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(userResources);
        }catch(Exception e) {
            return BadRequest(new { message = e.Message });
        }
    }
    
    /**
     * GET: iam/users/{profileId}/exists
     * <summary>
     *     Check if a user exists by profile id endpoint. It allows to check if a user exists by profile id
     * </summary>
     * <remarks>
     * Check if a user exists based on the profile id provided.&#xA;
     * It has the following property: &#xA;
     * <b>ProfileId</b>: User's profile id
     * </remarks>
     * <param name="profileId">The profile id</param>
     * <returns>True if the user exists, false otherwise</returns>
     * <response code="200">Returns true if the user exists, false otherwise&#xA;
     * Success Example:
     * true
     * </response>
     * <response code="500">If there was an internal error</response>
     */
    [HttpGet("{profileId:int}/exists")]
    [AllowAnonymous]
    public IActionResult IsUserExists([FromRoute] int profileId)
    {
        try {
            var isUsersExists = iamContextFacade.UsersExists(profileId);
            return Ok(isUsersExists);
        }catch(Exception e) {
            return BadRequest(new { message = e.Message });
        }

    }
    
    /**
     * GET: iam/users/get-username/{userId}
     * <summary>
     *     Get username by user id endpoint. It allows to get a username by user id
     * </summary>
     * <remarks>
     * Get a username based on the user id provided.&#xA;
     * It has the following property: &#xA;
     * <b>UserId</b>: User's id
     * </remarks>
     * <param name="userId">The user id</param>
     * <returns>The username</returns>
     * <response code="200">Returns the username&#xA;
     * Success Example:
     * "Pruebita"
     * </response>
     * <response code="204">No content</response>
     * <response code="500">If there was an internal error</response>
     */
    [HttpGet("get-username/{userId:int}")]
    public async Task<IActionResult> GetUsernameById(int userId)
    {
        try {
            var getUsernameByIdQuery = new GetUsernameByIdQuery(userId);
            var username = await userQueryService.Handle(getUsernameByIdQuery);
            return Ok(username);
        }catch(Exception e) {
            return BadRequest(new { message = e.Message });
        }
    }
    
    /**
     * PUT: iam/users/{userId}
     * <summary>
     *     Update user endpoint. It allows to update a user
     * </summary>
     * <remarks>
     * Update a user based on the parameters provided.&#xA;
     * It has the following properties: &#xA;
     * <b>Username</b>: User's name &#xA;
     * <b>Sample request:</b>
     * {
     *   "username": "Pruebita"
     * }
     * </remarks>
     * <param name="userId">The user id</param>
     * <param name="updateUsernameResource">The update username resource containing the new username</param>
     * <returns>The updated user resource</returns>
     * <response code="200">Returns the updated user resource&#xA;
     * Success Example:
     * {
     *   "id": 1,
     *   "username": "Pruebita"
     * }
     * </response>
     * <response code="400">Error updating user&#xA;
     * Error Example:
     * {
     *   "message": "User not found."
     * }
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="403">If the user is not authorized to access this resource</response>
     * <response code="500">If there was an internal error</response>
     */
    [HttpPut("{userId:int}")]
    public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUsernameResource updateUsernameResource)
    {
        try {
            var updateUserCommand =
                UpdateUsernameCommandFromResourceAssembler.ToUpdateUsernameCommand(userId, updateUsernameResource);
            var user = await userCommandService.Handle(updateUserCommand);
            var userResource = UserResourceFromEntityAssembler.ToResourceFromEntity(user!);
            return Ok(userResource);
        }catch(Exception e) {
            return BadRequest(new { message = e.Message });
        }
    }
}