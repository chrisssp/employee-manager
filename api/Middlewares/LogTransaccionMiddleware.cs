using EmpresaApi.Data;
using EmpresaApi.Models;

namespace EmpresaApi.Middlewares
{
    public class LogTransaccionMiddleware
    {
        private readonly RequestDelegate _next;

        public LogTransaccionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, EmpresaContext dbContext)
        {
            var verboHttp = context.Request.Method;
            var endpoint = context.Request.Path;

            context.Request.EnableBuffering();
            var bodyStream = new StreamReader(context.Request.Body);
            var payload = await bodyStream.ReadToEndAsync();
            context.Request.Body.Position = 0;

            await _next(context);

            var statusCode = context.Response.StatusCode;

            var log = new LogTransaccion
            {
                VerboHttp = verboHttp,
                Endpoint = endpoint,
                StatusCode = statusCode,
                Payload = string.IsNullOrEmpty(payload) ? "Sin payload" : payload
            };

            dbContext.Logs.Add(log);
            await dbContext.SaveChangesAsync();
        }
    }
}
