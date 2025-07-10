using System.Net.Mime;
using IAM.Domain.Services;
using IAM.Infrastructure.Pipeline.Middleware.Attributes;
using IAM.Interfaces.REST.Resources;
using IAM.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace IAM.Interfaces.REST;

/**
 * <summary>
 * Controller for managing user authentication operations.
 * </summary>
 * <remarks>
 * This controller provides endpoints for user sign-in and sign-up operations.
 * </remarks>
 */
[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class AuthenticationController(IUserCommandService userCommandService) : ControllerBase
{

    /**
     * POST: iam/authentication/sign-in
     * <summary>
     *     Sign in endpoint. It allows to authenticate a user
     * </summary>
     * <remarks>
     * Sign in with already registered user based on the parameters provided.&#xA;
     * It has the following properties: &#xA;
     * <b>Email</b>: User's email &#xA;
     * <b>Password</b>: User's password&#xA;
     * <b>Sample request:</b>
     * {
     *   "email": "prueba1@correo.com",
     *   "password": "Pruebita123!"
     * }
     * </remarks>
     * <param name="signInResource">The sign in resource containing username and password.</param>
     * <returns>The authenticated user resource, including a JWT token</returns>
     * <response code="200">Returns the authenticated user resource, including a JWT token&#xA;
     * Success Example:
     * {
     *   "id": 1,
     *   "username": "Pruebita",
     *   "token": "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE3NTI2MzcxODksImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL3NpZCI6IjEiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiUHJ1ZWJpdGEiLCJpYXQiOjE3NTIwMzIzODksIm5iZiI6MTc1MjAzMjM4OX0.fZhbDeq_hiv7UBdadGBvP06sszdBW8M2PWdDolJvmgs"
     * }
     * </response>
     * <response code="400">Error validating email or password&#xA;
     * Error Example:
     * {
     *   "message": "Invalid email or password"
     * }
     * </response>
     * <response code="500">If there was an internal error</response>
     */
    [HttpPost("sign-in")]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn([FromBody] SignInResource signInResource)
    {
        try {
            var signInCommand = SignInCommandFromResourceAssembler.ToCommandFromResource(signInResource);
            var authenticatedUser = await userCommandService.Handle(signInCommand);
            var resource =
                AuthenticatedUserResourceFromEntityAssembler.ToResourceFromEntity(authenticatedUser.user,
                    authenticatedUser.token);
            return Ok(resource);
        }catch(Exception e) {
            return BadRequest(new { message = e.Message });
        }
    }

    /**
     * POST: iam/authentication/sign-up
     * <summary>
     *     Sign up endpoint. It allows to create a new user
     * </summary>
     * <remarks>
     * Post (create) a new user based on the parameters provided.&#xA;
     * It has the following properties: &#xA;
     * <b>Email</b>: User's email &#xA;
     * <b>Username</b>: User's name &#xA;
     * <b>Password</b>: User's password&#xA;
     * <b>Sample request:</b>
     * {
     *   "email": "prueba1@correo.com",
     *   "username": "Prueba1",
     *   "password": "Pruebita123!"
     * }
     * </remarks>
     * <param name="signUpResource">The sign up resource containing username and password.</param>
     * <returns>A confirmation message on successful creation.</returns>
     * <response code="200">Returns a confirmation message on successful creation.&#xA;
     * Success Example:
     * {
     *   "message": "User created successfully"
     * }
     * </response>
     * <response code="400">Error with password, email or username&#xA;
     * Error Example:
     * {
     *   "message": "Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit and one special character"
     * }
     * </response>
     * <response code="500">If there was an internal error</response>
     */
    [HttpPost("sign-up")]
    [AllowAnonymous]
    public async Task<IActionResult> SignUp([FromBody] SignUpResource signUpResource)
    {
        try {
            var signUpCommand = SignUpCommandFromResourceAssembler.ToCommandFromResource(signUpResource);
            await userCommandService.Handle(signUpCommand);
            return Ok(new { message = "User created successfully"});
        }catch(Exception e) {
            return BadRequest(new { message = e.Message });
        }
    }
}