using System.Net.Mime;
using LocalManagement.Domain.Model.Queries;
using LocalManagement.Domain.Services;
using LocalManagement.Interfaces.REST.Resources;
using LocalManagement.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace LocalManagement.Interfaces.REST;

/**
 * <summary>
 * Controller for managing comments related to locals.
 * </summary>
 * <remarks>
 * This controller provides endpoints for creating and retrieving comments related to locals.
 * It allows users to add comments to locals and retrieve all comments for a specific local.
 * </remarks>
 */
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class CommentController(ICommentCommandService commandService, ICommentQueryService queryService) : ControllerBase
{
    /**
     * GET: local/comment/local/{localId}
     * <summary>
     *     Get all comments by local id endpoint. It allows to get all comments related to a specific local.
     * </summary>
     * <remarks>
     * Get all comments related to a specific local by its id.&#xA;
     * It has the following properties: &#xA;
     * <b>LocalId</b>: The ID of the local for which comments are being retrieved
     * </remarks>
     * <param name="localId">The local id</param>
     * <returns>A list of comment resources</returns>
     * <response code="200">Returns the list of comment resources&#xA;
     * Success Example:
     * [
     *   {
     *     "id": 1,
     *     "localId": 1,
     *     "text": "Great place!",
     *     "rating": 5
     *   },
     *   {
     *     "id": 2,
     *    "localId": 1,
     *    "text": "Not bad, but could be better.",
     *    "rating": 3
     *   }
     * ]
     * </response>
     * <response code="400">Error obtaining comments&#xA;
     * Error Example:
     * {
     *   "message": "Invalid local ID"
     * }
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="500">If there was an internal error</response>
     */
    [HttpGet("local/{localId:int}")]
    public async Task<IActionResult> GetAllCommentsByLocalId(int localId)
    {
        try {
            var getAllCommentsByLocalIdQuery = new GetAllCommentsByLocalIdQuery(localId);
            var comments = await queryService.Handle(getAllCommentsByLocalIdQuery);
            var commentsResources = comments.Select(CommentResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(commentsResources);
        }catch(Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
    }

    /**
     * POST: local/comment
     * <summary>
     *     Create a new comment endpoint. It allows to create a new comment related to a local.
     * </summary>
     * <remarks>
     * Post (Create) a new comment with the details provided in the request body.&#xA;
     * It has the following properties: &#xA;
     * <b>UserId</b>: The ID of the user creating the comment&#xA;
     * <b>LocalId</b>: The ID of the local being commented on&#xA;
     * <b>Text</b>: The content of the comment&#xA;
     * <b>Rating</b>: The rating given by the user (optional, can be null)&#xA;
     * <b>Sample request:</b>
     * {
     *    "userId": 1,
     *    "localId": 1,
     *    "text": "This is a great local!",
     *    "rating": 5
     * }
     * </remarks>
     * <param name="resource">The resource containing the comment details</param>
     * <returns>The created comment resource</returns>
     * <response code="201">Returns the created comment resource&#xA;
     * Success Example:
     * {
     *   "id": 1,
     *   "userId": 1,
     *   "localId": 1,
     *   "text": "This is a great local!",
     *   "rating": 5
     * }
     * </response>
     * <response code="400">Error creating comment&#xA;
     * Error Example:
     * {
     *   "message": "There is no locals matching the id specified"
     * }
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="403">If the user is not authorized to access this resource</response>
     * <response code="500">If there was an internal error</response>
     */
    [HttpPost]
    public async Task<IActionResult> CreateComment(CreateCommentResource resource)
    {
        try {
            var createCommentCommand = CreateCommentCommandFromResourceAssembler.ToCommandFromResource(resource);
            var comment = await commandService.Handle(createCommentCommand);
            if (comment is null) return BadRequest();
            var commentResource = CommentResourceFromEntityAssembler.ToResourceFromEntity(comment);
            return StatusCode(201,commentResource);
        }catch(Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
    }
}