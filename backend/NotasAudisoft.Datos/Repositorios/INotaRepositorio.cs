using NotasAudisoft.Entidades;

namespace NotasAudisoft.Datos.Repositorios;

public interface INotaRepositorio : IRepositorioGenerico<Nota>
{
    Task<IEnumerable<Nota>> ObtenerTodasConDetalleAsync();
    Task<Nota?> ObtenerPorIdConDetalleAsync(int id);
    Task<IEnumerable<Nota>> ObtenerPorEstudianteYMateriaAsync(int idEstudiante, int idMateria);
}
