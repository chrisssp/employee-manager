using EmpresaApi.Models;
using EmpresaApi.DTOs;
using EmpresaApi.Responses;

namespace EmpresaApi.Interfaces
{
    public interface ILogsService
    {
        Task<ApiResponse<List<LogTransacionDTO>>> ObtenerTodosAsync(int page = 1, int pageSize = 50);
        Task<ApiResponse<LogTransacionDTO>> ObtenerPorIdAsync(int id);
    }
}
