using EmpresaApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddLogging(config =>
{
    config.ClearProviders();
    config.AddConsole();
    config.AddDebug();
});

builder.Services.AddDbContext<EmpresaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<EmpresaApi.Interfaces.IEmpleadoService, EmpresaApi.Services.EmpleadoService>();
builder.Services.AddScoped<EmpresaApi.Interfaces.IRolesService, EmpresaApi.Services.RolesService>();
builder.Services.AddScoped<EmpresaApi.Interfaces.ILogsService, EmpresaApi.Services.LogsService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<EmpresaApi.Middlewares.ExceptionHandlingMiddleware>();

app.UseCors("PermitirFrontend");
app.UseAuthorization();
app.UseMiddleware<EmpresaApi.Middlewares.LogTransaccionMiddleware>();
app.MapControllers();

app.Run();
