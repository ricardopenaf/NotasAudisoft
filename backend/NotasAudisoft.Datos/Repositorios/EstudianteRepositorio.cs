using Microsoft.EntityFrameworkCore;
using NotasAudisoft.Entidades;

namespace NotasAudisoft.Datos.Repositorios;

public class EstudianteRepositorio : RepositorioGenerico<Estudiante>, IEstudianteRepositorio
{
    public EstudianteRepositorio(NotasDbContext contexto) : base(contexto)
    {
    }

    public async Task<bool> TieneNotasAsociadasAsync(int idEstudiante)
    {
        return await Contexto.Notas.AnyAsync(n => n.IdEstudiante == idEstudiante);
    }

    public async Task<bool> ExistePorNumeroIdentificacionAsync(long numeroIdentificacion, int? idExcluir = null)
    {
        return await Contexto.Estudiantes.AsNoTracking().AnyAsync(e =>
            e.NumeroIdentificacion == numeroIdentificacion && (idExcluir == null || e.Id != idExcluir));
    }
}
