using System.Linq.Expressions;

namespace NotasAudisoft.Datos.Repositorios;

public interface IRepositorioGenerico<TEntidad> where TEntidad : class
{
    Task<TEntidad?> ObtenerPorIdAsync(int id);
    Task<IEnumerable<TEntidad>> ObtenerTodosAsync();
    Task<(IEnumerable<TEntidad> Datos, int Total)> ObtenerPaginadoAsync(
        int numeroPagina,
        int tamanoPagina,
        Expression<Func<TEntidad, bool>>? filtro = null);
    Task<TEntidad> AgregarAsync(TEntidad entidad);
    Task ActualizarAsync(TEntidad entidad);
    Task EliminarAsync(TEntidad entidad);
    Task<bool> ExisteAsync(int id);
}
