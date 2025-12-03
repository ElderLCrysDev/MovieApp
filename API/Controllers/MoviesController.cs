using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace API.Controllers;

[ApiController]
[Route("api/movies")]
[Authorize]
public class MoviesController : ControllerBase
{
    private readonly IMovieService _movies; 

    public MoviesController(IMovieService movies) 
    {
        _movies = movies;
    }

    //-----BUSCA TOP FILMES POPULARES-----//
    [HttpGet("populares")]
    public async Task<IActionResult> Populares([FromQuery] int page = 1)
    {
        var resString = await _movies.GetPopularAsync(page);
        var resObj = JsonSerializer.Deserialize<JsonElement>(resString);
        return Ok(resObj);
    }

    //-----BUSCA FILMES-----//
    [HttpGet("buscar")]
    public async Task<IActionResult> Buscar([FromQuery] string query, [FromQuery] int page = 1)
    {
        if (string.IsNullOrWhiteSpace(query))
            return BadRequest(new { message = "query obrigatório" });

        var resString = await _movies.SearchAsync(query, page);
        var resObj = JsonSerializer.Deserialize<JsonElement>(resString);
        return Ok(resObj);
    }

    //-----DETALHAMENTO DO FILME-----//
    [HttpGet("{id}")]
    public async Task<IActionResult> Detalhes(int id)
    {
        var resString = await _movies.GetDetailsAsync(id);
        var resObj = JsonSerializer.Deserialize<JsonElement>(resString);
        return Ok(resObj);
    }

    //-----GET CREDITOS DO FILME-----//
    [HttpGet("{id}/creditos")]
    public async Task<IActionResult> Creditos(int id)
    {
        var resString = await _movies.GetCreditsAsync(id);
        var resObj = JsonSerializer.Deserialize<JsonElement>(resString);
        return Ok(resObj);
    }

    //-----GET TRAILER DO FILME-----//
    [HttpGet("{id}/videos")]
    public async Task<IActionResult> Videos(int id)
    {
        var resString = await _movies.GetVideosAsync(id);
        var resObj = JsonSerializer.Deserialize<JsonElement>(resString);
        return Ok(resObj);
    }
}
