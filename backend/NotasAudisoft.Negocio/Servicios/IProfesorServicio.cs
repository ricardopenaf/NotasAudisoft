using NotasAudisoft.Entidades.Comun;
using NotasAudisoft.Negocio.Dtos;

namespace NotasAudisoft.Negocio.Servicios;

public interface IProfesorServicio
{
    Task<ResultadoPaginado<ProfesorDto>> ObtenerPaginadoAsync(int numeroPagina, int tamanoPagina, string? busqueda);
    Task<ProfesorDto> ObtenerPorIdAsync(int id);
    Task<ProfesorDto> CrearAsync(ProfesorCrearDto dto);
    Task<ProfesorDto> ActualizarAsync(int id, ProfesorActualizarDto dto);
    Task EliminarAsync(int id);
}
