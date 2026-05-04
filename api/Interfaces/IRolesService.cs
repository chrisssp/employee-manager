using EmpresaApi.Models;
using EmpresaApi.DTOs;
using EmpresaApi.Responses;

namespace EmpresaApi.Interfaces
{
    public interface IRolesService
    {
        Task<List<Rol>> ObtenerTodosAsync();
        Task<ApiResponse<List<RolDTO>>> ObtenerPaginadoAsync(int page = 1, int pageSize = 10);
        Task<ApiResponse<RolDTO>> ObtenerPorIdAsync(int id);
        Task<ApiResponse<RolDTO>> CrearAsync(RolCreacionDTO dto);
        Task<ApiResponse<RolDTO>> ActualizarAsync(int id, RolCreacionDTO dto);
        Task<ApiResponse<object>> EliminarAsync(int id);
    }
}
