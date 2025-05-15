using System.Net.Mime;
using LocalManagement.Domain.Model.Queries;
using LocalManagement.Domain.Services;
using LocalManagement.Interfaces.REST.Resources;
using LocalManagement.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace LocalManagement.Interfaces.REST;


[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class CommentController(ICommentCommandService commandService, ICommentQueryService queryService) : ControllerBase
{
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