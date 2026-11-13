using NotasAudisoft.Entidades;

namespace NotasAudisoft.Datos.Repositorios;

public interface IEstudianteRepositorio : IRepositorioGenerico<Estudiante>
{
    Task<bool> TieneNotasAsociadasAsync(int idEstudiante);
    Task<bool> ExistePorNumeroIdentificacionAsync(long numeroIdentificacion, int? idExcluir = null);
}
