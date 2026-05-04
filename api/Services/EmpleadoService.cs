using EmpresaApi.Data;
using EmpresaApi.DTOs;
using EmpresaApi.Interfaces;
using EmpresaApi.Models;
using EmpresaApi.Responses;
using Microsoft.EntityFrameworkCore;

namespace EmpresaApi.Services
{
    public class EmpleadoService : IEmpleadoService
    {
        private readonly EmpresaContext _context;
        private readonly ILogger<EmpleadoService> _logger;

        public EmpleadoService(EmpresaContext context, ILogger<EmpleadoService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ApiResponse<IEnumerable<EmpleadoDTO>>> ObtenerTodosAsync()
        {
            try
            {
                var empleados = await _context.Empleados
                    .Include(e => e.Roles)
                    .Select(e => new EmpleadoDTO
                    {
                        Id = e.Id,
                        Nombre = e.Nombre,
                        ApellidoPaterno = e.ApellidoPaterno,
                        ApellidoMaterno = e.ApellidoMaterno,
                        Correo = e.Correo,
                        Activo = e.Activo,
                        FechaCreacion = e.FechaCreacion,
                        FechaActualizacion = e.FechaActualizacion,
                        FechaBaja = e.FechaBaja,
                        Roles = e.Roles.Select(r => r.Nombre).ToList()
                    })
                    .ToListAsync();

                return ApiResponse<IEnumerable<EmpleadoDTO>>.SuccessResponse(
                    empleados,
                    $"Se obtuvieron {empleados.Count} empleados exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener empleados");
                return ApiResponse<IEnumerable<EmpleadoDTO>>.ErrorResponse(
                    "Error al obtener los empleados", 500);
            }
        }

        public async Task<ApiResponse<List<EmpleadoDTO>>> ObtenerPaginadoAsync(int page = 1, int pageSize = 10)
        {
            try
            {
                var empleados = await _context.Empleados
                    .Include(e => e.Roles)
                    .OrderBy(e => e.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(e => new EmpleadoDTO
                    {
                        Id = e.Id,
                        Nombre = e.Nombre,
                        ApellidoPaterno = e.ApellidoPaterno,
                        ApellidoMaterno = e.ApellidoMaterno,
                        Correo = e.Correo,
                        Activo = e.Activo,
                        FechaCreacion = e.FechaCreacion,
                        FechaActualizacion = e.FechaActualizacion,
                        FechaBaja = e.FechaBaja,
                        Roles = e.Roles.Select(r => r.Nombre).ToList()
                    })
                    .ToListAsync();

                return ApiResponse<List<EmpleadoDTO>>.SuccessResponse(empleados);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener empleados paginados");
                return ApiResponse<List<EmpleadoDTO>>.ErrorResponse("Error al obtener los empleados", 500);
            }
        }

        public async Task<ApiResponse<EmpleadoDTO>> CrearAsync(EmpleadoCreacionDTO dto)
        {
            try
            {
                var correoExiste = await _context.Empleados.AnyAsync(e => e.Correo == dto.Correo);
                if (correoExiste)
                {
                    _logger.LogWarning("Intento de crear empleado con correo duplicado: {Correo}", dto.Correo);
                    return ApiResponse<EmpleadoDTO>.ErrorResponse(
                        $"Ya existe un empleado con el correo '{dto.Correo}'", 409);
                }

                var roles = await _context.Roles.Where(r => dto.RolesIds.Contains(r.Id)).ToListAsync();
                if (!roles.Any())
                {
                    _logger.LogWarning("Intento de crear empleado sin roles válidos");
                    return ApiResponse<EmpleadoDTO>.ErrorResponse(
                        "Los roles especificados no existen", 400);
                }

                var empleado = new Empleado
                {
                    Nombre = dto.Nombre,
                    ApellidoPaterno = dto.ApellidoPaterno,
                    ApellidoMaterno = dto.ApellidoMaterno,
                    Correo = dto.Correo,
                    Roles = roles
                };

                _context.Empleados.Add(empleado);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Empleado creado exitosamente: {Correo}", dto.Correo);

                var empleadoDto = new EmpleadoDTO
                {
                    Id = empleado.Id,
                    Nombre = empleado.Nombre,
                    ApellidoPaterno = empleado.ApellidoPaterno,
                    ApellidoMaterno = empleado.ApellidoMaterno,
                    Correo = empleado.Correo,
                    Activo = empleado.Activo,
                    FechaCreacion = empleado.FechaCreacion,
                    Roles = roles.Select(r => r.Nombre).ToList()
                };

                return ApiResponse<EmpleadoDTO>.CreatedResponse(empleadoDto, "Empleado creado exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear empleado");
                return ApiResponse<EmpleadoDTO>.ErrorResponse(
                    "Error al crear el empleado", 500);
            }
        }

        public async Task<ApiResponse<EmpleadoDTO>> ActualizarAsync(int id, EmpleadoCreacionDTO dto)
        {
            try
            {
                var empleado = await _context.Empleados
                    .Include(e => e.Roles)
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (empleado == null)
                {
                    _logger.LogWarning("Intento de actualizar empleado inexistente: {Id}", id);
                    return ApiResponse<EmpleadoDTO>.ErrorResponse(
                        $"Empleado con ID {id} no encontrado", 404);
                }

                if (empleado.Correo != dto.Correo)
                {
                    var correoExiste = await _context.Empleados
                        .AnyAsync(e => e.Correo == dto.Correo && e.Id != id);

                    if (correoExiste)
                    {
                        _logger.LogWarning("Intento de actualizar con correo duplicado: {Correo}", dto.Correo);
                        return ApiResponse<EmpleadoDTO>.ErrorResponse(
                            $"Ya existe otro empleado con el correo '{dto.Correo}'", 409);
                    }
                }

                var nuevosRoles = await _context.Roles
                    .Where(r => dto.RolesIds.Contains(r.Id))
                    .ToListAsync();

                if (!nuevosRoles.Any())
                {
                    _logger.LogWarning("Intento de actualizar empleado sin roles válidos: {Id}", id);
                    return ApiResponse<EmpleadoDTO>.ErrorResponse(
                        "Los roles especificados no existen", 400);
                }

                empleado.Nombre = dto.Nombre;
                empleado.ApellidoPaterno = dto.ApellidoPaterno;
                empleado.ApellidoMaterno = dto.ApellidoMaterno;
                empleado.Correo = dto.Correo;

                empleado.Roles.Clear();
                empleado.Roles.AddRange(nuevosRoles);

                await _context.SaveChangesAsync();

                _logger.LogInformation("Empleado actualizado exitosamente: {Id}", id);

                var empleadoDto = new EmpleadoDTO
                {
                    Id = empleado.Id,
                    Nombre = empleado.Nombre,
                    ApellidoPaterno = empleado.ApellidoPaterno,
                    ApellidoMaterno = empleado.ApellidoMaterno,
                    Correo = empleado.Correo,
                    Activo = empleado.Activo,
                    FechaCreacion = empleado.FechaCreacion,
                    FechaActualizacion = empleado.FechaActualizacion,
                    Roles = nuevosRoles.Select(r => r.Nombre).ToList()
                };

                return ApiResponse<EmpleadoDTO>.SuccessResponse(empleadoDto, "Empleado actualizado exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar empleado: {Id}", id);
                return ApiResponse<EmpleadoDTO>.ErrorResponse(
                    "Error al actualizar el empleado", 500);
            }
        }

        public async Task<ApiResponse<object>> DesactivarAsync(int id)
        {
            try
            {
                var empleado = await _context.Empleados.FindAsync(id);

                if (empleado == null)
                {
                    _logger.LogWarning("Intento de desactivar empleado inexistente: {Id}", id);
                    return ApiResponse<object>.ErrorResponse(
                        $"Empleado con ID {id} no encontrado", 404);
                }

                if (!empleado.Activo)
                {
                    _logger.LogWarning("Intento de desactivar empleado ya inactivo: {Id}", id);
                    return ApiResponse<object>.ErrorResponse(
                        "El empleado ya está desactivado", 400);
                }

                empleado.Activo = false;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Empleado desactivado exitosamente: {Id}", id);

                return ApiResponse<object>.SuccessResponse(
                    new { Id = id, Mensaje = "Desactivado" },
                    "Empleado desactivado exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al desactivar empleado: {Id}", id);
                return ApiResponse<object>.ErrorResponse(
                    "Error al desactivar el empleado", 500);
            }
        }

        public async Task<ApiResponse<object>> ReactivarAsync(int id)
        {
            try
            {
                var empleado = await _context.Empleados.FindAsync(id);

                if (empleado == null)
                {
                    _logger.LogWarning("Intento de reactivar empleado inexistente: {Id}", id);
                    return ApiResponse<object>.ErrorResponse($"Empleado con ID {id} no encontrado", 404);
                }

                if (empleado.Activo)
                {
                    _logger.LogWarning("Intento de reactivar empleado ya activo: {Id}", id);
                    return ApiResponse<object>.ErrorResponse("El empleado ya está activo", 400);
                }

                empleado.Activo = true;
                empleado.FechaBaja = null;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Empleado reactivado exitosamente: {Id}", id);

                return ApiResponse<object>.SuccessResponse(new { Id = id, Mensaje = "Reactivado" }, "Empleado reactivado exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al reactivar empleado: {Id}", id);
                return ApiResponse<object>.ErrorResponse("Error al reactivar el empleado", 500);
            }
        }
    }
}
