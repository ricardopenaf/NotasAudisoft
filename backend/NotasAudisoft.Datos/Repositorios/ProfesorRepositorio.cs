using Microsoft.EntityFrameworkCore;
using NotasAudisoft.Entidades;

namespace NotasAudisoft.Datos.Repositorios;

public class ProfesorRepositorio : RepositorioGenerico<Profesor>, IProfesorRepositorio
{
    public ProfesorRepositorio(NotasDbContext contexto) : base(contexto)
    {
    }

    public async Task<bool> TieneNotasAsociadasAsync(int idProfesor)
    {
        return await Contexto.Notas.AnyAsync(n => n.IdProfesor == idProfesor);
    }

    public async Task<bool> ExistePorNumeroIdentificacionAsync(long numeroIdentificacion, int? idExcluir = null)
    {
        return await Contexto.Profesores.AsNoTracking().AnyAsync(p =>
            p.NumeroIdentificacion == numeroIdentificacion && (idExcluir == null || p.Id != idExcluir));
    }
}
