using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize]
public class AdminController : ControllerBase
{
    private readonly ILogRepository _logRepo;

    public AdminController(ILogRepository logRepo)
    {
        _logRepo = logRepo;
    }

    [HttpGet("logs")]
    public async Task<IActionResult> GetLogs([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        //-----VALIDA NIVEL DE ADIM DO USUARIO-----//
        var tipo = User.Claims.FirstOrDefault(c => c.Type == "tipo")?.Value;
        if (tipo != "Administrador") return Forbid();

        var (items, total) = await _logRepo.GetPagedAsync(page, pageSize);
        return Ok(new { items, total });
    }
}
