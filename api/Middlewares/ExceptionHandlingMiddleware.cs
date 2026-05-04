using EmpresaApi.Responses;
using System.Text.Json;

namespace EmpresaApi.Middlewares
{
    /// <summary>
    /// Middleware global para manejar excepciones no capturadas
    /// Convierte cualquier error en una respuesta JSON consistente
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Excepción no manejada: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new ApiResponse<object>();

            switch (exception)
            {
                case ArgumentNullException argNullEx:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response = ApiResponse<object>.ErrorResponse(
                        $"Valor nulo no permitido: {argNullEx.ParamName}",
                        StatusCodes.Status400BadRequest);
                    break;

                case InvalidOperationException invalidOpEx:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response = ApiResponse<object>.ErrorResponse(
                        $"Operación inválida: {invalidOpEx.Message}",
                        StatusCodes.Status400BadRequest);
                    break;

                case Microsoft.EntityFrameworkCore.DbUpdateException dbEx:
                    context.Response.StatusCode = StatusCodes.Status409Conflict;
                    response = ApiResponse<object>.ErrorResponse(
                        "El registro ya existe o viola una restricción de base de datos. Verifique que el correo sea único.",
                        StatusCodes.Status409Conflict);
                    break;

                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    response = ApiResponse<object>.ErrorResponse(
                        "Error interno del servidor. Por favor, intente más tarde.",
                        StatusCodes.Status500InternalServerError);
                    break;
            }

            var jsonResponse = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(jsonResponse);
        }
    }
}
