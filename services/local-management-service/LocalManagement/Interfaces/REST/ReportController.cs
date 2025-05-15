using System.Net.Mime;
using LocalManagement.Domain.Model.Commands;
using LocalManagement.Domain.Model.Queries;
using LocalManagement.Domain.Services;
using LocalManagement.Interfaces.REST.Resources;
using LocalManagement.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace LocalManagement.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ReportController(IReportQueryService reportQueryService, IReportCommandService reportCommandService) : ControllerBase
{
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