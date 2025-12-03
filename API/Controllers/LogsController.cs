using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/logs")]
    public class LogsController : ControllerBase
    {
        private readonly ILogRepository _logRepo;

        public LogsController(ILogRepository logRepo)
        {
            _logRepo = logRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetLogs([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var (items, total) = await _logRepo.GetPagedAsync(page, pageSize);
            return Ok(new { total, items });
        }
    }
}
