using NotasAudisoft.Entidades.Comun;
using NotasAudisoft.Negocio.Dtos;

namespace NotasAudisoft.Negocio.Servicios;

public interface IMateriaServicio
{
    Task<ResultadoPaginado<MateriaDto>> ObtenerPaginadoAsync(int numeroPagina, int tamanoPagina, string? busqueda);
    Task<MateriaDto> ObtenerPorIdAsync(int id);
    Task<MateriaDto> CrearAsync(MateriaCrearDto dto);
    Task<MateriaDto> ActualizarAsync(int id, MateriaActualizarDto dto);
    Task EliminarAsync(int id);
}
