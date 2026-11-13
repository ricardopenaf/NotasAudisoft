using NotasAudisoft.Datos.Repositorios;
using NotasAudisoft.Entidades;
using NotasAudisoft.Entidades.Comun;
using NotasAudisoft.Negocio.Dtos;
using NotasAudisoft.Negocio.Excepciones;
using NotasAudisoft.Negocio.Validaciones;

namespace NotasAudisoft.Negocio.Servicios;

public class NotaServicio : INotaServicio
{
    private readonly INotaRepositorio _notaRepositorio;
    private readonly IEstudianteRepositorio _estudianteRepositorio;
    private readonly IProfesorRepositorio _profesorRepositorio;
    private readonly IMateriaRepositorio _materiaRepositorio;

    public NotaServicio(
        INotaRepositorio notaRepositorio,
        IEstudianteRepositorio estudianteRepositorio,
        IProfesorRepositorio profesorRepositorio,
        IMateriaRepositorio materiaRepositorio)
    {
        _notaRepositorio = notaRepositorio;
        _estudianteRepositorio = estudianteRepositorio;
        _profesorRepositorio = profesorRepositorio;
        _materiaRepositorio = materiaRepositorio;
    }

    public async Task<ResultadoPaginado<NotaDto>> ObtenerPaginadoAsync(int numeroPagina, int tamanoPagina)
    {
        numeroPagina = numeroPagina < 1 ? 1 : numeroPagina;
        tamanoPagina = tamanoPagina is < 1 or > 100 ? 10 : tamanoPagina;

        var todas = (await _notaRepositorio.ObtenerTodasConDetalleAsync())
            .OrderBy(n => n.Id)
            .ToList();

        var total = todas.Count;
        var pagina = todas
            .Skip((numeroPagina - 1) * tamanoPagina)
            .Take(tamanoPagina)
            .Select(MapearADto);

        return new ResultadoPaginado<NotaDto>
        {
            Datos = pagina,
            PaginaActual = numeroPagina,
            TamanoPagina = tamanoPagina,
            TotalRegistros = total
        };
    }

    public async Task<NotaDto> ObtenerPorIdAsync(int id)
    {
        var nota = await _notaRepositorio.ObtenerPorIdConDetalleAsync(id)
            ?? throw new ExcepcionNoEncontrado($"No se encontró la nota con Id {id}.");

        return MapearADto(nota);
    }

    /// <summary>
    /// Agrupa las notas por Estudiante + Materia y calcula el promedio,
    /// que es el valor que debe mostrarse como "Nota" en la vista principal.
    /// </summary>
    public async Task<ResultadoPaginado<NotaResumenDto>> ObtenerResumenPaginadoAsync(int numeroPagina, int tamanoPagina)
    {
        numeroPagina = numeroPagina < 1 ? 1 : numeroPagina;
        tamanoPagina = tamanoPagina is < 1 or > 100 ? 10 : tamanoPagina;

        var todas = await _notaRepositorio.ObtenerTodasConDetalleAsync();

        var agrupadas = todas
            .GroupBy(n => (n.IdEstudiante, n.IdMateria))
            .Select(g =>
            {
                var ultima = g.OrderByDescending(n => n.Id).First();
                return new NotaResumenDto
                {
                    IdEstudiante = g.Key.IdEstudiante,
                    NombreEstudiante = ultima.Estudiante?.NombreEstudiante ?? string.Empty,
                    IdMateria = g.Key.IdMateria,
                    NombreMateria = ultima.Materia?.NombreMateria ?? string.Empty,
                    IdProfesor = ultima.IdProfesor,
                    NombreProfesor = ultima.Profesor?.NombreProfesor ?? string.Empty,
                    PromedioNota = Math.Round(g.Average(n => n.ValorNota), 2),
                    CantidadNotas = g.Count()
                };
            })
            .OrderBy(r => r.NombreEstudiante)
            .ThenBy(r => r.NombreMateria)
            .ToList();

        var total = agrupadas.Count;
        var pagina = agrupadas
            .Skip((numeroPagina - 1) * tamanoPagina)
            .Take(tamanoPagina);

        return new ResultadoPaginado<NotaResumenDto>
        {
            Datos = pagina,
            PaginaActual = numeroPagina,
            TamanoPagina = tamanoPagina,
            TotalRegistros = total
        };
    }

    public async Task<IEnumerable<NotaDto>> ObtenerPorEstudianteYMateriaAsync(int idEstudiante, int idMateria)
    {
        var notas = await _notaRepositorio.ObtenerPorEstudianteYMateriaAsync(idEstudiante, idMateria);
        return notas.OrderBy(n => n.Id).Select(MapearADto);
    }

    public async Task<NotaDto> CrearAsync(NotaCrearDto dto)
    {
        await ValidarNotaAsync(dto.ValorNota, dto.IdEstudiante, dto.IdProfesor, dto.IdMateria);

        var nota = new Nota
        {
            IdEstudiante = dto.IdEstudiante,
            IdProfesor = dto.IdProfesor,
            IdMateria = dto.IdMateria,
            ValorNota = dto.ValorNota
        };

        var creada = await _notaRepositorio.AgregarAsync(nota);
        return await ObtenerPorIdAsync(creada.Id);
    }

    public async Task<NotaDto> ActualizarAsync(int id, NotaActualizarDto dto)
    {
        if (id != dto.Id)
        {
            throw new ExcepcionValidacion("El Id de la ruta no coincide con el Id del cuerpo de la solicitud.");
        }

        await ValidarNotaAsync(dto.ValorNota, dto.IdEstudiante, dto.IdProfesor, dto.IdMateria);

        var nota = await _notaRepositorio.ObtenerPorIdAsync(id)
            ?? throw new ExcepcionNoEncontrado($"No se encontró la nota con Id {id}.");

        nota.IdEstudiante = dto.IdEstudiante;
        nota.IdProfesor = dto.IdProfesor;
        nota.IdMateria = dto.IdMateria;
        nota.ValorNota = dto.ValorNota;

        await _notaRepositorio.ActualizarAsync(nota);
        return await ObtenerPorIdAsync(id);
    }

    public async Task EliminarAsync(int id)
    {
        var nota = await _notaRepositorio.ObtenerPorIdAsync(id)
            ?? throw new ExcepcionNoEncontrado($"No se encontró la nota con Id {id}.");

        await _notaRepositorio.EliminarAsync(nota);
    }

    private async Task ValidarNotaAsync(decimal valorNota, int idEstudiante, int idProfesor, int idMateria)
    {
        if (!ValidadorTexto.EsNotaValida(valorNota))
        {
            throw new ExcepcionValidacion("El valor de la nota debe estar entre 0 y 5.");
        }

        if (!await _estudianteRepositorio.ExisteAsync(idEstudiante))
        {
            throw new ExcepcionValidacion($"No existe un estudiante con Id {idEstudiante}.");
        }

        if (!await _profesorRepositorio.ExisteAsync(idProfesor))
        {
            throw new ExcepcionValidacion($"No existe un profesor con Id {idProfesor}.");
        }

        if (!await _materiaRepositorio.ExisteAsync(idMateria))
        {
            throw new ExcepcionValidacion($"No existe una materia con Id {idMateria}.");
        }
    }

    private static NotaDto MapearADto(Nota nota) => new()
    {
        Id = nota.Id,
        IdEstudiante = nota.IdEstudiante,
        NombreEstudiante = nota.Estudiante?.NombreEstudiante ?? string.Empty,
        IdProfesor = nota.IdProfesor,
        NombreProfesor = nota.Profesor?.NombreProfesor ?? string.Empty,
        IdMateria = nota.IdMateria,
        NombreMateria = nota.Materia?.NombreMateria ?? string.Empty,
        ValorNota = nota.ValorNota
    };
}
