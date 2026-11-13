using NotasAudisoft.Datos.Repositorios;
using NotasAudisoft.Entidades;
using NotasAudisoft.Entidades.Comun;
using NotasAudisoft.Negocio.Dtos;
using NotasAudisoft.Negocio.Excepciones;
using NotasAudisoft.Negocio.Validaciones;

namespace NotasAudisoft.Negocio.Servicios;

public class EstudianteServicio : IEstudianteServicio
{
    private readonly IEstudianteRepositorio _repositorio;

    public EstudianteServicio(IEstudianteRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<ResultadoPaginado<EstudianteDto>> ObtenerPaginadoAsync(int numeroPagina, int tamanoPagina, string? busqueda)
    {
        numeroPagina = numeroPagina < 1 ? 1 : numeroPagina;
        tamanoPagina = tamanoPagina is < 1 or > 100 ? 10 : tamanoPagina;

        var (datos, total) = await _repositorio.ObtenerPaginadoAsync(
            numeroPagina,
            tamanoPagina,
            string.IsNullOrWhiteSpace(busqueda) ? null : e => e.NombreEstudiante.Contains(busqueda));

        return new ResultadoPaginado<EstudianteDto>
        {
            Datos = datos.Select(MapearADto),
            PaginaActual = numeroPagina,
            TamanoPagina = tamanoPagina,
            TotalRegistros = total
        };
    }

    public async Task<EstudianteDto> ObtenerPorIdAsync(int id)
    {
        var estudiante = await _repositorio.ObtenerPorIdAsync(id)
            ?? throw new ExcepcionNoEncontrado($"No se encontró el estudiante con Id {id}.");

        return MapearADto(estudiante);
    }

    public async Task<EstudianteDto> CrearAsync(EstudianteCrearDto dto)
    {
        ValidarNombre(dto.NombreEstudiante);
        await ValidarNumeroIdentificacionAsync(dto.NumeroIdentificacion);

        var estudiante = new Estudiante
        {
            NombreEstudiante = dto.NombreEstudiante.Trim(),
            NumeroIdentificacion = dto.NumeroIdentificacion
        };
        var creado = await _repositorio.AgregarAsync(estudiante);

        return MapearADto(creado);
    }

    public async Task<EstudianteDto> ActualizarAsync(int id, EstudianteActualizarDto dto)
    {
        if (id != dto.Id)
        {
            throw new ExcepcionValidacion("El Id de la ruta no coincide con el Id del cuerpo de la solicitud.");
        }

        ValidarNombre(dto.NombreEstudiante);
        await ValidarNumeroIdentificacionAsync(dto.NumeroIdentificacion, idExcluir: id);

        var estudiante = await _repositorio.ObtenerPorIdAsync(id)
            ?? throw new ExcepcionNoEncontrado($"No se encontró el estudiante con Id {id}.");

        estudiante.NombreEstudiante = dto.NombreEstudiante.Trim();
        estudiante.NumeroIdentificacion = dto.NumeroIdentificacion;
        await _repositorio.ActualizarAsync(estudiante);

        return MapearADto(estudiante);
    }

    public async Task EliminarAsync(int id)
    {
        var estudiante = await _repositorio.ObtenerPorIdAsync(id)
            ?? throw new ExcepcionNoEncontrado($"No se encontró el estudiante con Id {id}.");

        if (await _repositorio.TieneNotasAsociadasAsync(id))
        {
            throw new ExcepcionNegocio("No se puede eliminar el estudiante porque tiene notas asociadas.");
        }

        await _repositorio.EliminarAsync(estudiante);
    }

    private static void ValidarNombre(string nombre)
    {
        if (!ValidadorTexto.EsNombreValido(nombre))
        {
            throw new ExcepcionValidacion(
                "El nombre del estudiante solo puede contener letras y espacios, sin números ni caracteres especiales.");
        }
    }

    private async Task ValidarNumeroIdentificacionAsync(long numeroIdentificacion, int? idExcluir = null)
    {
        if (!ValidadorTexto.EsNumeroIdentificacionValido(numeroIdentificacion))
        {
            throw new ExcepcionValidacion("El número de identificación debe ser un valor numérico positivo.");
        }

        if (await _repositorio.ExistePorNumeroIdentificacionAsync(numeroIdentificacion, idExcluir))
        {
            throw new ExcepcionValidacion(
                $"Ya existe un estudiante registrado con el número de identificación {numeroIdentificacion}.");
        }
    }

    private static EstudianteDto MapearADto(Estudiante estudiante) => new()
    {
        Id = estudiante.Id,
        NombreEstudiante = estudiante.NombreEstudiante,
        NumeroIdentificacion = estudiante.NumeroIdentificacion
    };
}
