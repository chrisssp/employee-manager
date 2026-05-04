using EmpresaApi.Data;
using EmpresaApi.Responses;
using EmpresaApi.Interfaces;
using EmpresaApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EmpresaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRolesService _rolesService;

        public RolesController(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<object>>>> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 0)
        {
            if (pageSize > 0)
            {
                var pag = await _rolesService.ObtenerPaginadoAsync(page, pageSize);
                if (!pag.Success) return StatusCode(pag.StatusCode ?? 500, pag);
                var dto = pag.Data.Select(r => new { r.Id, r.Nombre, r.Descripcion }).ToList();
                return Ok(ApiResponse<List<object>>.SuccessResponse(dto.Cast<object>().ToList()));
            }

            var roles = await _rolesService.ObtenerTodosAsync();
            var dtoAll = roles.Select(r => new { r.Id, r.Nombre, r.Descripcion }).ToList();
            return Ok(ApiResponse<List<object>>.SuccessResponse(dtoAll.Cast<object>().ToList()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<RolDTO>>> GetById(int id)
        {
            var result = await _rolesService.ObtenerPorIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<RolDTO>>> Create(RolCreacionDTO dto)
        {
            var result = await _rolesService.CrearAsync(dto);
            return result.Success ? CreatedAtAction(nameof(GetById), new { id = result.Data?.Id }, result) : BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<RolDTO>>> Update(int id, RolCreacionDTO dto)
        {
            var result = await _rolesService.ActualizarAsync(id, dto);
            return result.Success ? Ok(result) : (result.StatusCode == 404 ? NotFound(result) : (ActionResult)BadRequest(result));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
        {
            var result = await _rolesService.EliminarAsync(id);
            return result.Success ? Ok(result) : (result.StatusCode == 404 ? NotFound(result) : (ActionResult)BadRequest(result));
        }
    }
}
