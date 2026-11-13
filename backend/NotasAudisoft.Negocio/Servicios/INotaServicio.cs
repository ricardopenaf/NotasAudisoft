using NotasAudisoft.Entidades.Comun;
using NotasAudisoft.Negocio.Dtos;

namespace NotasAudisoft.Negocio.Servicios;

public interface INotaServicio
{
    Task<ResultadoPaginado<NotaDto>> ObtenerPaginadoAsync(int numeroPagina, int tamanoPagina);
    Task<NotaDto> ObtenerPorIdAsync(int id);
    Task<ResultadoPaginado<NotaResumenDto>> ObtenerResumenPaginadoAsync(int numeroPagina, int tamanoPagina);
    Task<IEnumerable<NotaDto>> ObtenerPorEstudianteYMateriaAsync(int idEstudiante, int idMateria);
    Task<NotaDto> CrearAsync(NotaCrearDto dto);
    Task<NotaDto> ActualizarAsync(int id, NotaActualizarDto dto);
    Task EliminarAsync(int id);
}
