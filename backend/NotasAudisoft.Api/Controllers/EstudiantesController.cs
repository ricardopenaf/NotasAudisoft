using Microsoft.AspNetCore.Mvc;
using NotasAudisoft.Negocio.Dtos;
using NotasAudisoft.Negocio.Servicios;

namespace NotasAudisoft.Api.Controllers;

[ApiController]
[Route("api/estudiantes")]
[Produces("application/json")]
public class EstudiantesController : ControllerBase
{
    private readonly IEstudianteServicio _servicio;

    public EstudiantesController(IEstudianteServicio servicio)
    {
        _servicio = servicio;
    }

    /// <summary>Obtiene los estudiantes de forma paginada.</summary>
    [HttpGet]
    public async Task<IActionResult> ObtenerPaginado(
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanoPagina = 10,
        [FromQuery] string? busqueda = null)
    {
        var resultado = await _servicio.ObtenerPaginadoAsync(pagina, tamanoPagina, busqueda);
        return Ok(resultado);
    }

    /// <summary>Obtiene un estudiante por su Id.</summary>
    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var estudiante = await _servicio.ObtenerPorIdAsync(id);
        return Ok(estudiante);
    }

    /// <summary>Crea un nuevo estudiante.</summary>
    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] EstudianteCrearDto dto)
    {
        var creado = await _servicio.CrearAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, creado);
    }

    /// <summary>Actualiza un estudiante existente.</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] EstudianteActualizarDto dto)
    {
        var actualizado = await _servicio.ActualizarAsync(id, dto);
        return Ok(actualizado);
    }

    /// <summary>Elimina un estudiante.</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        await _servicio.EliminarAsync(id);
        return NoContent();
    }
}
