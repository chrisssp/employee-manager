using EmpresaApi.Interfaces;
using EmpresaApi.DTOs;
using EmpresaApi.Responses;
using Microsoft.AspNetCore.Mvc;

namespace EmpresaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly ILogsService _logsService;

        public LogsController(ILogsService logsService)
        {
            _logsService = logsService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<LogTransacionDTO>>>> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 50)
        {
            var result = await _logsService.ObtenerTodosAsync(page, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<LogTransacionDTO>>> GetById(int id)
        {
            var result = await _logsService.ObtenerPorIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}
