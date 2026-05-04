using EmpresaApi.Data;
using EmpresaApi.Interfaces;
using EmpresaApi.Models;
using EmpresaApi.DTOs;
using EmpresaApi.Responses;
using Microsoft.EntityFrameworkCore;

namespace EmpresaApi.Services
{
    public class RolesService : IRolesService
    {
        private readonly EmpresaContext _context;
        private readonly ILogger<RolesService> _logger;

        public RolesService(EmpresaContext context, ILogger<RolesService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Rol>> ObtenerTodosAsync()
        {
            return await _context.Roles
                .Select(r => new Rol { Id = r.Id, Nombre = r.Nombre, Descripcion = r.Descripcion })
                .ToListAsync();
        }

        public async Task<ApiResponse<List<RolDTO>>> ObtenerPaginadoAsync(int page = 1, int pageSize = 10)
        {
            try
            {
                var roles = await _context.Roles
                    .OrderBy(r => r.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .Select(r => new RolDTO { Id = r.Id, Nombre = r.Nombre, Descripcion = r.Descripcion })
                    .ToListAsync();

                return ApiResponse<List<RolDTO>>.SuccessResponse(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener roles paginados");
                return ApiResponse<List<RolDTO>>.ErrorResponse("Error al obtener roles", 500);
            }
        }

        public async Task<ApiResponse<RolDTO>> ObtenerPorIdAsync(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null)
            {
                _logger.LogWarning("Intento de obtener rol inexistente: {Id}", id);
                return ApiResponse<RolDTO>.ErrorResponse("Rol no encontrado", 404);
            }

            return ApiResponse<RolDTO>.SuccessResponse(new RolDTO
            {
                Id = rol.Id,
                Nombre = rol.Nombre,
                Descripcion = rol.Descripcion
            });
        }

        public async Task<ApiResponse<RolDTO>> CrearAsync(RolCreacionDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombre))
            {
                return ApiResponse<RolDTO>.ErrorResponse("El nombre del rol es obligatorio");
            }

            var rolExistente = await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == dto.Nombre);
            if (rolExistente != null)
            {
                _logger.LogWarning("Intento de crear rol duplicado: {Nombre}", dto.Nombre);
                return ApiResponse<RolDTO>.ErrorResponse("Ya existe un rol con ese nombre", 400);
            }

            var rol = new Rol
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion
            };

            _context.Roles.Add(rol);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Rol creado: {Id} - {Nombre}", rol.Id, rol.Nombre);

            return ApiResponse<RolDTO>.CreatedResponse(new RolDTO
            {
                Id = rol.Id,
                Nombre = rol.Nombre,
                Descripcion = rol.Descripcion
            });
        }

        public async Task<ApiResponse<RolDTO>> ActualizarAsync(int id, RolCreacionDTO dto)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null)
            {
                _logger.LogWarning("Intento de actualizar rol inexistente: {Id}", id);
                return ApiResponse<RolDTO>.ErrorResponse("Rol no encontrado", 404);
            }

            if (string.IsNullOrWhiteSpace(dto.Nombre))
            {
                return ApiResponse<RolDTO>.ErrorResponse("El nombre del rol es obligatorio");
            }

            // Verificar si el nombre ya existe en otro rol
            var rolExistente = await _context.Roles
                .FirstOrDefaultAsync(r => r.Nombre == dto.Nombre && r.Id != id);
            if (rolExistente != null)
            {
                _logger.LogWarning("Intento de actualizar rol a nombre duplicado: {Nombre}", dto.Nombre);
                return ApiResponse<RolDTO>.ErrorResponse("Ya existe otro rol con ese nombre", 400);
            }

            rol.Nombre = dto.Nombre;
            rol.Descripcion = dto.Descripcion;

            _context.Roles.Update(rol);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Rol actualizado: {Id} - {Nombre}", rol.Id, rol.Nombre);

            return ApiResponse<RolDTO>.SuccessResponse(new RolDTO
            {
                Id = rol.Id,
                Nombre = rol.Nombre,
                Descripcion = rol.Descripcion
            }, "Rol actualizado exitosamente");
        }

        public async Task<ApiResponse<object>> EliminarAsync(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null)
            {
                _logger.LogWarning("Intento de eliminar rol inexistente: {Id}", id);
                return ApiResponse<object>.ErrorResponse("Rol no encontrado", 404);
            }

            // Verificar si hay empleados con este rol
            var empleadosConRol = await _context.Empleados
                .Where(e => e.Roles.Any(r => r.Id == id))
                .CountAsync();

            if (empleadosConRol > 0)
            {
                _logger.LogWarning("Intento de eliminar rol con empleados: {Id}", id);
                return ApiResponse<object>.ErrorResponse(
                    $"No se puede eliminar este rol. Hay {empleadosConRol} empleado(s) asignado(s) a este rol", 400);
            }

            _context.Roles.Remove(rol);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Rol eliminado: {Id} - {Nombre}", rol.Id, rol.Nombre);

            return ApiResponse<object>.SuccessResponse(new { }, "Rol eliminado exitosamente");
        }
    }
}

