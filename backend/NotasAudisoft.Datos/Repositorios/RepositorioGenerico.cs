using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace NotasAudisoft.Datos.Repositorios;

public class RepositorioGenerico<TEntidad> : IRepositorioGenerico<TEntidad> where TEntidad : class
{
    protected readonly NotasDbContext Contexto;
    protected readonly DbSet<TEntidad> ConjuntoEntidades;

    public RepositorioGenerico(NotasDbContext contexto)
    {
        Contexto = contexto;
        ConjuntoEntidades = contexto.Set<TEntidad>();
    }

    public virtual async Task<TEntidad?> ObtenerPorIdAsync(int id)
    {
        return await ConjuntoEntidades.FindAsync(id);
    }

    public virtual async Task<IEnumerable<TEntidad>> ObtenerTodosAsync()
    {
        return await ConjuntoEntidades.AsNoTracking().ToListAsync();
    }

    public virtual async Task<(IEnumerable<TEntidad> Datos, int Total)> ObtenerPaginadoAsync(
        int numeroPagina,
        int tamanoPagina,
        Expression<Func<TEntidad, bool>>? filtro = null)
    {
        IQueryable<TEntidad> consulta = ConjuntoEntidades.AsNoTracking();

        if (filtro is not null)
        {
            consulta = consulta.Where(filtro);
        }

        var total = await consulta.CountAsync();

        var datos = await consulta
            .Skip((numeroPagina - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .ToListAsync();

        return (datos, total);
    }

    public virtual async Task<TEntidad> AgregarAsync(TEntidad entidad)
    {
        await ConjuntoEntidades.AddAsync(entidad);
        await Contexto.SaveChangesAsync();
        return entidad;
    }

    public virtual async Task ActualizarAsync(TEntidad entidad)
    {
        ConjuntoEntidades.Update(entidad);
        await Contexto.SaveChangesAsync();
    }

    public virtual async Task EliminarAsync(TEntidad entidad)
    {
        ConjuntoEntidades.Remove(entidad);
        await Contexto.SaveChangesAsync();
    }

    public virtual async Task<bool> ExisteAsync(int id)
    {
        return await ConjuntoEntidades.AsNoTracking().AnyAsync(e => EF.Property<int>(e, "Id") == id);
    }
}
