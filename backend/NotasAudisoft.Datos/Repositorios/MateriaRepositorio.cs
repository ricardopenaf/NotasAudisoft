using Microsoft.EntityFrameworkCore;
using NotasAudisoft.Entidades;

namespace NotasAudisoft.Datos.Repositorios;

public class MateriaRepositorio : RepositorioGenerico<Materia>, IMateriaRepositorio
{
    public MateriaRepositorio(NotasDbContext contexto) : base(contexto)
    {
    }

    public async Task<bool> TieneNotasAsociadasAsync(int idMateria)
    {
        return await Contexto.Notas.AnyAsync(n => n.IdMateria == idMateria);
    }
}
