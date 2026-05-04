using EmpresaApi.Data;
using EmpresaApi.Interfaces;
using EmpresaApi.DTOs;
using EmpresaApi.Responses;
using Microsoft.EntityFrameworkCore;

namespace EmpresaApi.Services
{
    public class LogsService : ILogsService
    {
        private readonly EmpresaContext _context;

        public LogsService(EmpresaContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<List<LogTransacionDTO>>> ObtenerTodosAsync(int page = 1, int pageSize = 50)
        {
            var logs = await _context.Logs
                .OrderByDescending(l => l.Fecha)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(l => new LogTransacionDTO
                {
                    Id = l.Id,
                    VerboHttp = l.VerboHttp,
                    Endpoint = l.Endpoint,
                    StatusCode = l.StatusCode,
                    Payload = l.Payload,
                    Fecha = new DateTimeOffset(DateTime.SpecifyKind(l.Fecha, DateTimeKind.Utc))
                })
                .ToListAsync();

            return ApiResponse<List<LogTransacionDTO>>.SuccessResponse(logs);
        }

        public async Task<ApiResponse<LogTransacionDTO>> ObtenerPorIdAsync(int id)
        {
            var log = await _context.Logs.FindAsync(id);
            if (log == null)
                return ApiResponse<LogTransacionDTO>.ErrorResponse("Log no encontrado", 404);

            return ApiResponse<LogTransacionDTO>.SuccessResponse(new LogTransacionDTO
            {
                Id = log.Id,
                VerboHttp = log.VerboHttp,
                Endpoint = log.Endpoint,
                StatusCode = log.StatusCode,
                Payload = log.Payload,
                Fecha = new DateTimeOffset(DateTime.SpecifyKind(log.Fecha, DateTimeKind.Utc))
            });
        }
    }
}
