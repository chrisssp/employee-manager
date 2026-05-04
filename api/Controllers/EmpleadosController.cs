using EmpresaApi.DTOs;
using EmpresaApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmpresaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {
        private readonly IEmpleadoService _empleadoService;

        public EmpleadosController(IEmpleadoService empleadoService)
        {
            _empleadoService = empleadoService;
        }

        /// <summary>
        /// Crea un nuevo empleado
        /// </summary>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        public async Task<IActionResult> CrearEmpleado(EmpleadoCreacionDTO dto)
        {
            var resultado = await _empleadoService.CrearAsync(dto);

            if (!resultado.Success)
            {
                return StatusCode(resultado.StatusCode ?? 400, resultado);
            }

            return StatusCode(resultado.StatusCode ?? 201, resultado);
        }

        /// <summary>
        /// Obtiene todos los empleados
        /// </summary>
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> ObtenerEmpleados([FromQuery] int page = 1, [FromQuery] int pageSize = 0)
        {
            if (pageSize > 0)
            {
                var resultadoPaginado = await _empleadoService.ObtenerPaginadoAsync(page, pageSize);
                return Ok(resultadoPaginado);
            }

            var resultado = await _empleadoService.ObtenerTodosAsync();
            return Ok(resultado);
        }

        /// <summary>
        /// Actualiza un empleado existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ActualizarEmpleado(int id, EmpleadoCreacionDTO dto)
        {
            var resultado = await _empleadoService.ActualizarAsync(id, dto);

            if (!resultado.Success)
            {
                return StatusCode(resultado.StatusCode ?? 400, resultado);
            }

            return Ok(resultado);
        }

        /// <summary>
        /// Desactiva un empleado (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> BajaEmpleado(int id)
        {
            var resultado = await _empleadoService.DesactivarAsync(id);

            if (!resultado.Success)
            {
                return StatusCode(resultado.StatusCode ?? 400, resultado);
            }

            return Ok(resultado);
        }

        /// <summary>
        /// Reactiva un empleado previamente desactivado
        /// </summary>
        [HttpPost("{id}/reactivar")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ReactivarEmpleado(int id)
        {
            var resultado = await _empleadoService.ReactivarAsync(id);

            if (!resultado.Success)
            {
                return StatusCode(resultado.StatusCode ?? 400, resultado);
            }

            return Ok(resultado);
        }
    }
}
