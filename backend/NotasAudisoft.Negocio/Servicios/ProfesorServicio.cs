using NotasAudisoft.Datos.Repositorios;
using NotasAudisoft.Entidades;
using NotasAudisoft.Entidades.Comun;
using NotasAudisoft.Negocio.Dtos;
using NotasAudisoft.Negocio.Excepciones;
using NotasAudisoft.Negocio.Validaciones;

namespace NotasAudisoft.Negocio.Servicios;

public class ProfesorServicio : IProfesorServicio
{
    private readonly IProfesorRepositorio _repositorio;

    public ProfesorServicio(IProfesorRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<ResultadoPaginado<ProfesorDto>> ObtenerPaginadoAsync(int numeroPagina, int tamanoPagina, string? busqueda)
    {
        numeroPagina = numeroPagina < 1 ? 1 : numeroPagina;
        tamanoPagina = tamanoPagina is < 1 or > 100 ? 10 : tamanoPagina;

        var (datos, total) = await _repositorio.ObtenerPaginadoAsync(
            numeroPagina,
            tamanoPagina,
            string.IsNullOrWhiteSpace(busqueda) ? null : p => p.NombreProfesor.Contains(busqueda));

        return new ResultadoPaginado<ProfesorDto>
        {
            Datos = datos.Select(MapearADto),
            PaginaActual = numeroPagina,
            TamanoPagina = tamanoPagina,
            TotalRegistros = total
        };
    }

    public async Task<ProfesorDto> ObtenerPorIdAsync(int id)
    {
        var profesor = await _repositorio.ObtenerPorIdAsync(id)
            ?? throw new ExcepcionNoEncontrado($"No se encontró el profesor con Id {id}.");

        return MapearADto(profesor);
    }

    public async Task<ProfesorDto> CrearAsync(ProfesorCrearDto dto)
    {
        ValidarNombre(dto.NombreProfesor);
        await ValidarNumeroIdentificacionAsync(dto.NumeroIdentificacion);

        var profesor = new Profesor
        {
            NombreProfesor = dto.NombreProfesor.Trim(),
            NumeroIdentificacion = dto.NumeroIdentificacion
        };
        var creado = await _repositorio.AgregarAsync(profesor);

        return MapearADto(creado);
    }

    public async Task<ProfesorDto> ActualizarAsync(int id, ProfesorActualizarDto dto)
    {
        if (id != dto.Id)
        {
            throw new ExcepcionValidacion("El Id de la ruta no coincide con el Id del cuerpo de la solicitud.");
        }

        ValidarNombre(dto.NombreProfesor);
        await ValidarNumeroIdentificacionAsync(dto.NumeroIdentificacion, idExcluir: id);

        var profesor = await _repositorio.ObtenerPorIdAsync(id)
            ?? throw new ExcepcionNoEncontrado($"No se encontró el profesor con Id {id}.");

        profesor.NombreProfesor = dto.NombreProfesor.Trim();
        profesor.NumeroIdentificacion = dto.NumeroIdentificacion;
        await _repositorio.ActualizarAsync(profesor);

        return MapearADto(profesor);
    }

    public async Task EliminarAsync(int id)
    {
        var profesor = await _repositorio.ObtenerPorIdAsync(id)
            ?? throw new ExcepcionNoEncontrado($"No se encontró el profesor con Id {id}.");

        if (await _repositorio.TieneNotasAsociadasAsync(id))
        {
            throw new ExcepcionNegocio("No se puede eliminar el profesor porque tiene notas asociadas.");
        }

        await _repositorio.EliminarAsync(profesor);
    }

    private static void ValidarNombre(string nombre)
    {
        if (!ValidadorTexto.EsNombreValido(nombre))
        {
            throw new ExcepcionValidacion(
                "El nombre del profesor solo puede contener letras y espacios, sin números ni caracteres especiales.");
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
                $"Ya existe un profesor registrado con el número de identificación {numeroIdentificacion}.");
        }
    }

    private static ProfesorDto MapearADto(Profesor profesor) => new()
    {
        Id = profesor.Id,
        NombreProfesor = profesor.NombreProfesor,
        NumeroIdentificacion = profesor.NumeroIdentificacion
    };
}
