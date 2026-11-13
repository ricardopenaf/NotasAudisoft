using Microsoft.AspNetCore.Mvc;
using NotasAudisoft.Negocio.Dtos;
using NotasAudisoft.Negocio.Servicios;

namespace NotasAudisoft.Api.Controllers;

[ApiController]
[Route("api/profesores")]
[Produces("application/json")]
public class ProfesoresController : ControllerBase
{
    private readonly IProfesorServicio _servicio;

    public ProfesoresController(IProfesorServicio servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public async Task<IActionResult> ObtenerPaginado(
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanoPagina = 10,
        [FromQuery] string? busqueda = null)
    {
        var resultado = await _servicio.ObtenerPaginadoAsync(pagina, tamanoPagina, busqueda);
        return Ok(resultado);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerPorId(int id)
    {
        var profesor = await _servicio.ObtenerPorIdAsync(id);
        return Ok(profesor);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] ProfesorCrearDto dto)
    {
        var creado = await _servicio.CrearAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, creado);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] ProfesorActualizarDto dto)
    {
        var actualizado = await _servicio.ActualizarAsync(id, dto);
        return Ok(actualizado);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        await _servicio.EliminarAsync(id);
        return NoContent();
    }
}
