using Microsoft.AspNetCore.Mvc;
using NotasAudisoft.Negocio.Dtos;
using NotasAudisoft.Negocio.Servicios;

namespace NotasAudisoft.Api.Controllers;

[ApiController]
[Route("api/notas")]
[Produces("application/json")]
public class NotasController : ControllerBase
{
    private readonly INotaServicio _servicio;

    public NotasController(INotaServicio servicio)
    {
        _servicio = servicio;
    }

    /// <summary>Lista individual de notas, paginada.</summary>
    [HttpGet]
    public async Task<IActionResult> ObtenerPaginado([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 10)
    {
        var resultado = await _servicio.ObtenerPaginadoAsync(pagina, tamanoPagina);
        return Ok(resultado);
    }

    /// <summary>
    /// Resumen paginado agrupado por Estudiante + Materia, mostrando el promedio de las notas.
    /// Es la vista principal del formulario de Notas.
    /// </summary>
    [HttpGet("resumen")]
    public async Task<IActionResult> ObtenerResumenPaginado([FromQuery] int pagina = 1, [FromQuery] int tamanoPagina = 10)
    {
        var resultado = await _servicio.ObtenerResumenPaginadoAsync(pagina, tamanoPagina);
        return Ok(resultado);
    }

    /// <summary>Lista las notas individuales de un estudiante en una materia (botón "Mostrar Notas").</summary>
    [HttpGet("estudiante/{idEstudiante:int}/materia/{idMateria:int}")]
    public async Task<IActionResult> ObtenerPorEstudianteYMateria(int idEstudiante, int idMateria)
    {
        var notas = await _servicio.ObtenerPorEstudianteYMateriaAsync(idEstudiante, idMateria);
        return Ok(notas);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var nota = await _servicio.ObtenerPorIdAsync(id);
        return Ok(nota);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] NotaCrearDto dto)
    {
        var creada = await _servicio.CrearAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creada.Id }, creada);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] NotaActualizarDto dto)
    {
        var actualizada = await _servicio.ActualizarAsync(id, dto);
        return Ok(actualizada);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        await _servicio.EliminarAsync(id);
        return NoContent();
    }
}
