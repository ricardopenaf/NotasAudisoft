using NotasAudisoft.Entidades.Comun;
using NotasAudisoft.Negocio.Dtos;

namespace NotasAudisoft.Negocio.Servicios;

public interface IEstudianteServicio
{
    Task<ResultadoPaginado<EstudianteDto>> ObtenerPaginadoAsync(int numeroPagina, int tamanoPagina, string? busqueda);
    Task<EstudianteDto> ObtenerPorIdAsync(int id);
    Task<EstudianteDto> CrearAsync(EstudianteCrearDto dto);
    Task<EstudianteDto> ActualizarAsync(int id, EstudianteActualizarDto dto);
    Task EliminarAsync(int id);
}
