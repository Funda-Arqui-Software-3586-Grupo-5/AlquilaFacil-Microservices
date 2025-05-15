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
     * <summary>
     *     Get user by id endpoint. It allows to get a user by id
     * </summary>
     * <param name="userId">The user id</param>
     * <returns>The user resource</returns>
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
     * <summary>
     *     Get all users endpoint. It allows to get all users
     * </summary>
     * <returns>The user resources</returns>
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