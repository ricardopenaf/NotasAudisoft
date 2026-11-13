using NotasAudisoft.Entidades;

namespace NotasAudisoft.Datos.Repositorios;

public interface IMateriaRepositorio : IRepositorioGenerico<Materia>
{
    Task<bool> TieneNotasAsociadasAsync(int idMateria);
}
