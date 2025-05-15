using System.Net.Mime;
using LocalManagement.Domain.Model.Queries;
using LocalManagement.Domain.Services;
using LocalManagement.Interfaces.REST.Resources;
using LocalManagement.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LocalManagement.Interfaces.REST;


[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class LocalsController(ILocalCommandService localCommandService, ILocalQueryService localQueryService)
:ControllerBase
{
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