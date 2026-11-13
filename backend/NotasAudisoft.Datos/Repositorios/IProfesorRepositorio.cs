using NotasAudisoft.Entidades;

namespace NotasAudisoft.Datos.Repositorios;

public interface IProfesorRepositorio : IRepositorioGenerico<Profesor>
{
    Task<bool> TieneNotasAsociadasAsync(int idProfesor);
    Task<bool> ExistePorNumeroIdentificacionAsync(long numeroIdentificacion, int? idExcluir = null);
}
