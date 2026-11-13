using Microsoft.AspNetCore.Mvc;
using NotasAudisoft.Negocio.Dtos;
using NotasAudisoft.Negocio.Servicios;

namespace NotasAudisoft.Api.Controllers;

[ApiController]
[Route("api/materias")]
[Produces("application/json")]
public class MateriasController : ControllerBase
{
    private readonly IMateriaServicio _servicio;

    public MateriasController(IMateriaServicio servicio)
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
        var materia = await _servicio.ObtenerPorIdAsync(id);
        return Ok(materia);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] MateriaCrearDto dto)
    {
        var creada = await _servicio.CrearAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creada.Id }, creada);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, [FromBody] MateriaActualizarDto dto)
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
