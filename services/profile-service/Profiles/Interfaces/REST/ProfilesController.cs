using System.Net.Mime;
using AlquilaFacilPlatform.Profiles.Domain.Model.Queries;
using AlquilaFacilPlatform.Profiles.Domain.Services;
using AlquilaFacilPlatform.Profiles.Interfaces.REST.Resources;
using AlquilaFacilPlatform.Profiles.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Profiles.Interfaces.REST.Transform;

namespace AlquilaFacilPlatform.Profiles.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class ProfilesController(
    IProfileCommandService profileCommandService, 
    IProfileQueryService profileQueryService)
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateProfile([FromBody] CreateProfileResource createProfileResource)
    {
        var createProfileCommand = CreateProfileCommandFromResourceAssembler.ToCommandFromResource(createProfileResource);
        var profile = await profileCommandService.Handle(createProfileCommand);
        if (profile is null) return BadRequest();
        var resource = ProfileResourceFromEntityAssembler.ToResourceFromEntity(profile);
        return CreatedAtAction(nameof(GetProfileById), new {profileId = resource.Id}, resource);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllProfiles()
    {
        var getAllProfilesQuery = new GetAllProfilesQuery();
        var profiles = await profileQueryService.Handle(getAllProfilesQuery);
        var resources = profiles.Select(ProfileResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
    
    [HttpGet("{profileId}")]
    public async Task<IActionResult> GetProfileById(int profileId)
    {
        var profile = await profileQueryService.Handle(new GetProfileByIdQuery(profileId));
        if (profile == null) return NotFound();
        var profileResource = ProfileResourceFromEntityAssembler.ToResourceFromEntity(profile);
        return Ok(profileResource);
    }
    
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetProfileByUserId(int userId)
    {
        var profile = await profileQueryService.Handle(new GetProfileByUserIdQuery(userId));
        if (profile == null) return NotFound();
        var profileResource = ProfileResourceFromEntityAssembler.ToResourceFromEntity(profile);
        return Ok(profileResource);
    }
    
    [HttpGet("is-user-subscribed/{userId}")]
    public async Task<IActionResult> IsUserSubscribed(int userId)
    {
        var query = new IsUserSubscribeQuery(userId);
        var user = await profileQueryService.Handle(query);
        return Ok(user);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateFarm(int id,[FromBody] UpdateProfileResource updateProfileResource)
    {
        var updateProfileCommand = UpdateProfileCommandFromResourceAssembler.ToCommandFromResource(updateProfileResource,id);
        var result = await profileCommandService.Handle(updateProfileCommand);
        return Ok(ProfileResourceFromEntityAssembler.ToResourceFromEntity(result));
    }
}