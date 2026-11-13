using System.Net;
using System.Text.Json;
using NotasAudisoft.Negocio.Excepciones;

namespace NotasAudisoft.Api.Middleware;

public class MiddlewareManejoErrores
{
    private readonly RequestDelegate _siguiente;
    private readonly ILogger<MiddlewareManejoErrores> _logger;

    public MiddlewareManejoErrores(RequestDelegate siguiente, ILogger<MiddlewareManejoErrores> logger)
    {
        _siguiente = siguiente;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext contexto)
    {
        try
        {
            await _siguiente(contexto);
        }
        catch (Exception excepcion)
        {
            await ManejarExcepcionAsync(contexto, excepcion);
        }
    }

    private async Task ManejarExcepcionAsync(HttpContext contexto, Exception excepcion)
    {
        var (codigo, titulo) = excepcion switch
        {
            ExcepcionNoEncontrado => (HttpStatusCode.NotFound, "Recurso no encontrado"),
            ExcepcionValidacion => (HttpStatusCode.BadRequest, "Error de validación"),
            ExcepcionNegocio => (HttpStatusCode.BadRequest, "Error de negocio"),
            _ => (HttpStatusCode.InternalServerError, "Error interno del servidor")
        };

        if (codigo == HttpStatusCode.InternalServerError)
        {
            _logger.LogError(excepcion, "Error no controlado al procesar la solicitud {Ruta}", contexto.Request.Path);
        }

        contexto.Response.ContentType = "application/json";
        contexto.Response.StatusCode = (int)codigo;

        var respuesta = new
        {
            titulo,
            estado = (int)codigo,
            mensaje = codigo == HttpStatusCode.InternalServerError
                ? "Ocurrió un error inesperado. Intente nuevamente más tarde."
                : excepcion.Message
        };

        await contexto.Response.WriteAsync(JsonSerializer.Serialize(respuesta));
    }
}
