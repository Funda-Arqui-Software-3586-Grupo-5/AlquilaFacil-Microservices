using System.Net.Mime;
using LocalManagement.Domain.Model.Queries;
using LocalManagement.Domain.Services;
using LocalManagement.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;

namespace LocalManagement.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class LocalCategoriesController(ILocalCategoryQueryService localCategoryQueryService)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllLocalCategories()
    {
        try {
            var getAllLocalCategoriesQuery = new GetAllLocalCategoriesQuery();
            var localCategories = await localCategoryQueryService.Handle(getAllLocalCategoriesQuery);
            var localCategoryResources = localCategories.Select(LocalCategoryResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(localCategoryResources);
        }catch(Exception ex) {
            return BadRequest(new { message = ex.Message });
        }
    }
}
