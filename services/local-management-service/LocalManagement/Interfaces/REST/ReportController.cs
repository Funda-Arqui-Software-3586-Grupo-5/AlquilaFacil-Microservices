using System.Net.Mime;
using LocalManagement.Domain.Model.Commands;
using LocalManagement.Domain.Model.Queries;
using LocalManagement.Domain.Services;
using LocalManagement.Interfaces.REST.Resources;
using LocalManagement.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace LocalManagement.Interfaces.REST;

/**
 * <summary>
 * Controller for managing reports related to locals.
 * </summary>
 * <remarks>
 * This controller provides endpoints for creating, retrieving, and deleting reports related to locals.
 * It allows users to report issues with locals and manage those reports.
 * </remarks>
 */
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ReportController(IReportQueryService reportQueryService, IReportCommandService reportCommandService) : ControllerBase
{
    /**
     * POST: local/report
     * <summary>
     *     Create a report endpoint. It allows to create a report related to a local.
     * </summary>
     * <remarks>
     * This endpoint allows users to create a report for a local.&#xA;
     * The report includes details such as the local ID, user ID, title, and description.&#xA;
     * It has the following properties:&#xA;
     * <b>LocalId</b>: The ID of the local being reported&#xA;
     * <b>UserId</b>: The ID of the user creating the report&#xA;
     * <b>Title</b>: A brief title for the report&#xA;
     * <b>Description</b>: A detailed description of the issue being reported&#xA;
     * <b>Sample request:</b>
     * {
     *   "localId": 1,
     *   "userId": 1,
     *   "title": "Inappropriate content",
     *   "description": "This local has inappropriate content."
     * }
     * </remarks>
     * <param name="createReportResource">The resource containing the report details.</param>
     * <returns>The created report resource</returns>
     * <response code="201">Returns the created report resource&#xA;
     * Success Example:
     * {
     *   "id": 1,
     *   "localId": 1,
     *   "userId": 1,
     *   "title": "Inappropriate content",
     *   "description": "This local has inappropriate content."
     * }
     * </response>
     * <response code="400">Error creating report&#xA;
     * Error Example:
     * {
     *   "message": "Local ID or user ID not found"
     * }
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="403">If the user is not authorized to access this resource</response>
     * <response code="500">If there was an internal error</response>
     */
    [HttpPost]
    public async Task<IActionResult> CreateReport([FromBody] CreateReportResource createReportResource)
    {
        try {
            var command = CreateReportCommandFromResourceAssembler.ToCommandFromResource(createReportResource);
            var report = await reportCommandService.Handle(command);
            var reportResource = ReportResourceFromEntityAssembler.ToResourceFromEntity(report);
            return StatusCode(201, reportResource);
        }catch(Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    /**
     * GET: local/report/get-by-user-id/{userId}
     * <summary>
     *     Get reports by user ID endpoint. It allows to retrieve all reports created by a specific user.
     * </summary>
     * <remarks>
     * This endpoint retrieves all reports created by a specific user.&#xA;
     * It has the following property:&#xA;
     * <b>UserId</b>: The ID of the user whose reports are being retrieved
     * </remarks>
     * <param name="userId">The user ID</param>
     * <returns>A list of report resources created by the specified user</returns>
     * <response code="200">Returns the list of report resources&#xA;
     * Success Example:
     * [
     *   {
     *     "id": 1,
     *     "localId": 1,
     *     "userId": 1,
     *     "title": "Inappropriate content",
     *     "description": "This local has inappropriate content."
     *   },
     *   {
     *     "id": 2,
     *     "localId": 2,
     *     "userId": 1,
     *     "title": "Spam",
     *     "description": "This local is spam."
     *   }
     * ]
     * </response>
     * <response code="400">Error obtaining reports&#xA;
     * Error Example:
     * {
     *   "message": "Error retrieving reports"
     * }
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="500">If there was an internal error</response>
     */
    [HttpGet("get-by-user-id/{userId:int}")]
    public async Task<IActionResult> GetReportsByUserId(int userId)
    {
        try {
            var query = new GetReportsByUserIdQuery(userId);
            var reports = await reportQueryService.Handle(query);
            var reportResources = reports.Select(ReportResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(reportResources);
        }catch(Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    /**
     * GET: local/report/get-by-local-id/{localId}
     * <summary>
     *     Get reports by local ID endpoint. It allows to retrieve all reports related to a specific local.
     * </summary>
     * <remarks>
     * This endpoint retrieves all reports related to a specific local.&#xA;
     * It has the following property:&#xA;
     * <b>LocalId</b>: The ID of the local whose reports are being retrieved
     * </remarks>
     * <param name="localId">The local ID</param>
     * <returns>A list of report resources related to the specified local</returns>
     * <response code="200">Returns the list of report resources&#xA;
     * Success Example:
     * [
     *   {
     *     "id": 1,
     *     "localId": 1,
     *     "userId": 1,
     *     "title": "Inappropriate content",
     *     "description": "This local has inappropriate content."
     *   },
     *   {
     *     "id": 2,
     *     "localId": 1,
     *     "userId": 2,
     *     "title": "Spam",
     *     "description": "This local is spam."
     *   }
     * ]
     * </response>
     * <response code="400">Error obtaining reports&#xA;
     * Error Example:
     * {
     *   "message": "Error retrieving reports"
     * }
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="500">If there was an internal error</response>
     */
    [HttpGet("get-by-local-id/{localId:int}")]
    public async Task<IActionResult> GetReportsByLocalId(int localId)
    {
        try {    
            var query = new GetReportsByLocalIdQuery(localId);
            var reports = await reportQueryService.Handle(query);
            var reportResources = reports.Select(ReportResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(reportResources);
        }catch(Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    /**
     * DELETE: local/report/{reportId}
     * <summary>
     *     Delete report endpoint. It allows to delete a report by its ID.
     * </summary>
     * <remarks>
     * This endpoint allows users to delete a report by its ID.&#xA;
     * It has the following property:&#xA;
     * <b>ReportId</b>: The ID of the report to be deleted
     * </remarks>
     * <param name="reportId">The report ID</param>
     * <returns>The deleted report resource</returns>
     * <response code="200">Returns the deleted report resource&#xA;
     * Success Example:
     * {
     *   "id": 1,
     *   "localId": 1,
     *   "userId": 1,
     *   "title": "Inappropriate content",
     *   "description": "This local has inappropriate content.",
     *   "createdAt": "2025-07-10T16:34:57.057304"
     * }
     * </response>
     * <response code="400">Error deleting report&#xA;
     * Error Example:
     * {
     *   "message": "Report not found"
     * }
     * </response>
     * <response code="401">If the user is not authenticated</response>
     * <response code="403">If the user is not authorized to access this resource</response>
     * <response code="500">If there was an internal error</response>
     */
    [HttpDelete("{reportId:int}")]
    public async Task<IActionResult> DeleteReport(int reportId)
    {
        try {
            var command = new DeleteReportCommand(reportId);
            var reportDeleted = await reportCommandService.Handle(command);
            return StatusCode(200, reportDeleted);
        }catch(Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
    }
}