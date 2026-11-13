using Microsoft.EntityFrameworkCore;
using NotasAudisoft.Entidades;

namespace NotasAudisoft.Datos.Repositorios;

public class NotaRepositorio : RepositorioGenerico<Nota>, INotaRepositorio
{
    public NotaRepositorio(NotasDbContext contexto) : base(contexto)
    {
    }

    public async Task<Nota?> ObtenerPorIdConDetalleAsync(int id)
    {
        return await Contexto.Notas
            .Include(n => n.Estudiante)
            .Include(n => n.Profesor)
            .Include(n => n.Materia)
            .AsNoTracking()
            .FirstOrDefaultAsync(n => n.Id == id);
    }

    public async Task<IEnumerable<Nota>> ObtenerTodasConDetalleAsync()
    {
        return await Contexto.Notas
            .Include(n => n.Estudiante)
            .Include(n => n.Profesor)
            .Include(n => n.Materia)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Nota>> ObtenerPorEstudianteYMateriaAsync(int idEstudiante, int idMateria)
    {
        return await Contexto.Notas
            .Include(n => n.Estudiante)
            .Include(n => n.Profesor)
            .Include(n => n.Materia)
            .AsNoTracking()
            .Where(n => n.IdEstudiante == idEstudiante && n.IdMateria == idMateria)
            .ToListAsync();
    }
}
