using AEXMovies.Repositories.Exceptions;
using AEXMovies.Services.ActorService;
using AEXMovies.Services.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AEXMovies.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ActorsController : Controller
{
    private readonly IActorService _actorService;

    public ActorsController(IActorService actorService)
    {
        _actorService = actorService;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var actor = await _actorService.Get(id);
        if (actor == null)
            return NotFound($"Actor with ID = {id} not found");
        return Ok(actor);
    }

    [Authorize(Roles = "Admin,Moderator")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateActorDto dto)
    {
        var actor = await _actorService.Create(dto);
        return Ok(actor);
    }
    
    [Authorize(Roles = "Admin,Moderator")]
    [HttpPatch("{id}")]
    public async Task<IActionResult> Update(int id, UpdateActorDto dto)
    {
        try
        {
            var actor = await _actorService.Update(id, dto);
            return Ok(actor);
        }
        catch (RecordNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}