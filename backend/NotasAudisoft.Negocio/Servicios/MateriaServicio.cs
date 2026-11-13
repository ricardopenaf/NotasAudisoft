using NotasAudisoft.Datos.Repositorios;
using NotasAudisoft.Entidades;
using NotasAudisoft.Entidades.Comun;
using NotasAudisoft.Negocio.Dtos;
using NotasAudisoft.Negocio.Excepciones;
using NotasAudisoft.Negocio.Validaciones;

namespace NotasAudisoft.Negocio.Servicios;

public class MateriaServicio : IMateriaServicio
{
    private readonly IMateriaRepositorio _repositorio;

    public MateriaServicio(IMateriaRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<ResultadoPaginado<MateriaDto>> ObtenerPaginadoAsync(int numeroPagina, int tamanoPagina, string? busqueda)
    {
        numeroPagina = numeroPagina < 1 ? 1 : numeroPagina;
        tamanoPagina = tamanoPagina is < 1 or > 100 ? 10 : tamanoPagina;

        var (datos, total) = await _repositorio.ObtenerPaginadoAsync(
            numeroPagina,
            tamanoPagina,
            string.IsNullOrWhiteSpace(busqueda) ? null : m => m.NombreMateria.Contains(busqueda));

        return new ResultadoPaginado<MateriaDto>
        {
            Datos = datos.Select(MapearADto),
            PaginaActual = numeroPagina,
            TamanoPagina = tamanoPagina,
            TotalRegistros = total
        };
    }

    public async Task<MateriaDto> ObtenerPorIdAsync(int id)
    {
        var materia = await _repositorio.ObtenerPorIdAsync(id)
            ?? throw new ExcepcionNoEncontrado($"No se encontró la materia con Id {id}.");

        return MapearADto(materia);
    }

    public async Task<MateriaDto> CrearAsync(MateriaCrearDto dto)
    {
        ValidarNombre(dto.NombreMateria);

        var materia = new Materia { NombreMateria = dto.NombreMateria.Trim() };
        var creada = await _repositorio.AgregarAsync(materia);

        return MapearADto(creada);
    }

    public async Task<MateriaDto> ActualizarAsync(int id, MateriaActualizarDto dto)
    {
        if (id != dto.Id)
        {
            throw new ExcepcionValidacion("El Id de la ruta no coincide con el Id del cuerpo de la solicitud.");
        }

        ValidarNombre(dto.NombreMateria);

        var materia = await _repositorio.ObtenerPorIdAsync(id)
            ?? throw new ExcepcionNoEncontrado($"No se encontró la materia con Id {id}.");

        materia.NombreMateria = dto.NombreMateria.Trim();
        await _repositorio.ActualizarAsync(materia);

        return MapearADto(materia);
    }

    public async Task EliminarAsync(int id)
    {
        var materia = await _repositorio.ObtenerPorIdAsync(id)
            ?? throw new ExcepcionNoEncontrado($"No se encontró la materia con Id {id}.");

        if (await _repositorio.TieneNotasAsociadasAsync(id))
        {
            throw new ExcepcionNegocio("No se puede eliminar la materia porque tiene notas asociadas.");
        }

        await _repositorio.EliminarAsync(materia);
    }

    private static void ValidarNombre(string nombre)
    {
        if (!ValidadorTexto.EsNombreValido(nombre))
        {
            throw new ExcepcionValidacion(
                "El nombre de la materia solo puede contener letras y espacios, sin números ni caracteres especiales.");
        }
    }

    private static MateriaDto MapearADto(Materia materia) => new()
    {
        Id = materia.Id,
        NombreMateria = materia.NombreMateria
    };
}
