using NotasAudisoft.Api.Middleware;
using NotasAudisoft.Datos;
using NotasAudisoft.Negocio;

var builder = WebApplication.CreateBuilder(args);

const string PoliticaCors = "PoliticaCorsNotasAudisoft";

builder.Services.AddControllers()
    .AddJsonOptions(opciones =>
    {
        opciones.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opciones =>
{
    opciones.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Notas Audisoft API",
        Version = "v1",
        Description = "API REST para la gestión de Estudiantes, Profesores, Materias y Notas."
    });
});

builder.Services.AgregarCapaDatos(builder.Configuration);
builder.Services.AgregarCapaNegocio();

var origenesPermitidos = builder.Configuration.GetSection("CorsConfiguracion:OrigenesPermitidos").Get<string[]>()
    ?? new[] { "http://localhost:4200" };

builder.Services.AddCors(opciones =>
{
    opciones.AddPolicy(PoliticaCors, politica =>
    {
        politica.WithOrigins(origenesPermitidos)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(opciones =>
{
    opciones.SwaggerEndpoint("/swagger/v1/swagger.json", "Notas Audisoft API v1");
});

app.UseMiddleware<MiddlewareManejoErrores>();

app.UseCors(PoliticaCors);

app.UseAuthorization();

app.MapControllers();

app.Run();
