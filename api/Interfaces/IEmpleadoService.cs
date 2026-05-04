using EmpresaApi.DTOs;
using EmpresaApi.Responses;

namespace EmpresaApi.Interfaces
{
    public interface IEmpleadoService
    {
        Task<ApiResponse<IEnumerable<EmpleadoDTO>>> ObtenerTodosAsync();
        Task<ApiResponse<List<EmpleadoDTO>>> ObtenerPaginadoAsync(int page = 1, int pageSize = 10);
        Task<ApiResponse<EmpleadoDTO>> CrearAsync(EmpleadoCreacionDTO dto);
        Task<ApiResponse<EmpleadoDTO>> ActualizarAsync(int id, EmpleadoCreacionDTO dto);
        Task<ApiResponse<object>> DesactivarAsync(int id);
        Task<ApiResponse<object>> ReactivarAsync(int id);
    }
}
