using NotasAudisoft.Datos.Repositorios;
using NotasAudisoft.Negocio.Dtos;
using NotasAudisoft.Negocio.Excepciones;
using NotasAudisoft.Negocio.Servicios;
using NotasAudisoft.Tests.Utilidades;
using Xunit;

namespace NotasAudisoft.Tests.Negocio;

public class EstudianteServicioPruebas
{
    private static EstudianteServicio CrearServicio(out IEstudianteRepositorio repositorio)
    {
        var contexto = FabricaContextoPrueba.CrearContexto();
        repositorio = new EstudianteRepositorio(contexto);
        return new EstudianteServicio(repositorio);
    }

    [Fact]
    public async Task CrearAsync_ConNombreValido_CreaElEstudiante()
    {
        var servicio = CrearServicio(out _);

        var creado = await servicio.CrearAsync(new EstudianteCrearDto { NombreEstudiante = "María José Restrepo", NumeroIdentificacion = 5001 });

        Assert.True(creado.Id > 0);
        Assert.Equal("María José Restrepo", creado.NombreEstudiante);
        Assert.Equal(5001, creado.NumeroIdentificacion);
    }

    [Theory]
    [InlineData("Pedro123")]
    [InlineData("Pedro@Pérez")]
    [InlineData("Pedro_Pérez")]
    [InlineData("12345")]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("A")]
    public async Task CrearAsync_ConNombreInvalido_LanzaExcepcionValidacion(string nombreInvalido)
    {
        var servicio = CrearServicio(out _);

        await Assert.ThrowsAsync<ExcepcionValidacion>(() =>
            servicio.CrearAsync(new EstudianteCrearDto { NombreEstudiante = nombreInvalido, NumeroIdentificacion = 5002 }));
    }

    [Theory]
    [InlineData("José Ángel Núñez")]
    [InlineData("Ana María")]
    [InlineData("Peña")]
    public async Task CrearAsync_ConNombreConTildesYEnies_CreaElEstudiante(string nombreValido)
    {
        var servicio = CrearServicio(out _);

        var creado = await servicio.CrearAsync(new EstudianteCrearDto { NombreEstudiante = nombreValido, NumeroIdentificacion = 5003 });

        Assert.Equal(nombreValido, creado.NombreEstudiante);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public async Task CrearAsync_ConNumeroIdentificacionInvalido_LanzaExcepcionValidacion(long numeroInvalido)
    {
        var servicio = CrearServicio(out _);

        await Assert.ThrowsAsync<ExcepcionValidacion>(() =>
            servicio.CrearAsync(new EstudianteCrearDto { NombreEstudiante = "Camilo Rojas", NumeroIdentificacion = numeroInvalido }));
    }

    [Fact]
    public async Task CrearAsync_ConNumeroIdentificacionYaExistente_LanzaExcepcionValidacion()
    {
        var servicio = CrearServicio(out _);
        await servicio.CrearAsync(new EstudianteCrearDto { NombreEstudiante = "Camilo Rojas", NumeroIdentificacion = 5004 });

        var excepcion = await Assert.ThrowsAsync<ExcepcionValidacion>(() =>
            servicio.CrearAsync(new EstudianteCrearDto { NombreEstudiante = "Otro Estudiante", NumeroIdentificacion = 5004 }));

        Assert.Contains("5004", excepcion.Message);
    }

    [Fact]
    public async Task CrearAsync_ConNumeroIdentificacionYaUsadoPorSemilla_LanzaExcepcionValidacion()
    {
        var servicio = CrearServicio(out _);

        await Assert.ThrowsAsync<ExcepcionValidacion>(() =>
            servicio.CrearAsync(new EstudianteCrearDto { NombreEstudiante = "Otro Estudiante", NumeroIdentificacion = 1001 }));
    }

    [Fact]
    public async Task ObtenerPorIdAsync_ConIdInexistente_LanzaExcepcionNoEncontrado()
    {
        var servicio = CrearServicio(out _);

        await Assert.ThrowsAsync<ExcepcionNoEncontrado>(() => servicio.ObtenerPorIdAsync(9999));
    }

    [Fact]
    public async Task ActualizarAsync_ConIdDeRutaDistintoAlDelCuerpo_LanzaExcepcionValidacion()
    {
        var servicio = CrearServicio(out _);
        var creado = await servicio.CrearAsync(new EstudianteCrearDto { NombreEstudiante = "Camilo Rojas", NumeroIdentificacion = 5005 });

        await Assert.ThrowsAsync<ExcepcionValidacion>(() =>
            servicio.ActualizarAsync(creado.Id, new EstudianteActualizarDto { Id = creado.Id + 1, NombreEstudiante = "Camilo Rojas", NumeroIdentificacion = 5005 }));
    }

    [Fact]
    public async Task ActualizarAsync_ConDatosValidos_ActualizaElNombre()
    {
        var servicio = CrearServicio(out _);
        var creado = await servicio.CrearAsync(new EstudianteCrearDto { NombreEstudiante = "Camilo Rojas", NumeroIdentificacion = 5006 });

        var actualizado = await servicio.ActualizarAsync(
            creado.Id,
            new EstudianteActualizarDto { Id = creado.Id, NombreEstudiante = "Camilo Andrés Rojas", NumeroIdentificacion = 5006 });

        Assert.Equal("Camilo Andrés Rojas", actualizado.NombreEstudiante);
    }

    [Fact]
    public async Task ActualizarAsync_ConservandoSuPropioNumeroIdentificacion_NoLanzaExcepcion()
    {
        var servicio = CrearServicio(out _);
        var creado = await servicio.CrearAsync(new EstudianteCrearDto { NombreEstudiante = "Camilo Rojas", NumeroIdentificacion = 5007 });

        var actualizado = await servicio.ActualizarAsync(
            creado.Id,
            new EstudianteActualizarDto { Id = creado.Id, NombreEstudiante = "Camilo Rojas", NumeroIdentificacion = 5007 });

        Assert.Equal(5007, actualizado.NumeroIdentificacion);
    }

    [Fact]
    public async Task ActualizarAsync_ConNumeroIdentificacionDeOtroEstudiante_LanzaExcepcionValidacion()
    {
        var servicio = CrearServicio(out _);
        await servicio.CrearAsync(new EstudianteCrearDto { NombreEstudiante = "Camilo Rojas", NumeroIdentificacion = 5008 });
        var segundo = await servicio.CrearAsync(new EstudianteCrearDto { NombreEstudiante = "Otro Estudiante", NumeroIdentificacion = 5009 });

        await Assert.ThrowsAsync<ExcepcionValidacion>(() =>
            servicio.ActualizarAsync(segundo.Id, new EstudianteActualizarDto { Id = segundo.Id, NombreEstudiante = "Otro Estudiante", NumeroIdentificacion = 5008 }));
    }

    [Fact]
    public async Task EliminarAsync_ConEstudianteSinNotas_EliminaCorrectamente()
    {
        var servicio = CrearServicio(out _);
        var creado = await servicio.CrearAsync(new EstudianteCrearDto { NombreEstudiante = "Camilo Rojas", NumeroIdentificacion = 5010 });

        await servicio.EliminarAsync(creado.Id);

        await Assert.ThrowsAsync<ExcepcionNoEncontrado>(() => servicio.ObtenerPorIdAsync(creado.Id));
    }

    [Fact]
    public async Task EliminarAsync_ConEstudianteConNotasAsociadas_LanzaExcepcionNegocio()
    {
        var contexto = FabricaContextoPrueba.CrearContexto();
        var repositorio = new EstudianteRepositorio(contexto);
        var servicio = new EstudianteServicio(repositorio);

        await Assert.ThrowsAsync<ExcepcionNegocio>(() => servicio.EliminarAsync(1));
    }

    [Fact]
    public async Task ObtenerPaginadoAsync_RetornaElTamanoDePaginaSolicitado()
    {
        var servicio = CrearServicio(out _);
        await servicio.CrearAsync(new EstudianteCrearDto { NombreEstudiante = "Estudiante Uno", NumeroIdentificacion = 5011 });
        await servicio.CrearAsync(new EstudianteCrearDto { NombreEstudiante = "Estudiante Dos", NumeroIdentificacion = 5012 });

        var resultado = await servicio.ObtenerPaginadoAsync(numeroPagina: 1, tamanoPagina: 2, busqueda: null);

        Assert.Equal(2, resultado.Datos.Count());
        Assert.Equal(1, resultado.PaginaActual);
        Assert.Equal(2, resultado.TamanoPagina);
    }
}
