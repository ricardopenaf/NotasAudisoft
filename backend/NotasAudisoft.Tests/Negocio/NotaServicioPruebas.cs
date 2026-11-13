using NotasAudisoft.Datos.Repositorios;
using NotasAudisoft.Negocio.Dtos;
using NotasAudisoft.Negocio.Excepciones;
using NotasAudisoft.Negocio.Servicios;
using NotasAudisoft.Tests.Utilidades;
using Xunit;

namespace NotasAudisoft.Tests.Negocio;

public class NotaServicioPruebas
{
    private static NotaServicio CrearServicio()
    {
        var contexto = FabricaContextoPrueba.CrearContexto();
        var notaRepositorio = new NotaRepositorio(contexto);
        var estudianteRepositorio = new EstudianteRepositorio(contexto);
        var profesorRepositorio = new ProfesorRepositorio(contexto);
        var materiaRepositorio = new MateriaRepositorio(contexto);

        return new NotaServicio(notaRepositorio, estudianteRepositorio, profesorRepositorio, materiaRepositorio);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2.5)]
    [InlineData(5)]
    public async Task CrearAsync_ConNotaDentroDelRango_CreaLaNota(decimal valorNota)
    {
        var servicio = CrearServicio();

        var creada = await servicio.CrearAsync(new NotaCrearDto
        {
            IdEstudiante = 1,
            IdProfesor = 1,
            IdMateria = 1,
            ValorNota = valorNota
        });

        Assert.Equal(valorNota, creada.ValorNota);
    }

    [Theory]
    [InlineData(-0.1)]
    [InlineData(5.1)]
    [InlineData(10)]
    [InlineData(-5)]
    public async Task CrearAsync_ConNotaFueraDelRango_LanzaExcepcionValidacion(decimal valorInvalido)
    {
        var servicio = CrearServicio();

        await Assert.ThrowsAsync<ExcepcionValidacion>(() => servicio.CrearAsync(new NotaCrearDto
        {
            IdEstudiante = 1,
            IdProfesor = 1,
            IdMateria = 1,
            ValorNota = valorInvalido
        }));
    }

    [Fact]
    public async Task CrearAsync_ConEstudianteInexistente_LanzaExcepcionValidacion()
    {
        var servicio = CrearServicio();

        await Assert.ThrowsAsync<ExcepcionValidacion>(() => servicio.CrearAsync(new NotaCrearDto
        {
            IdEstudiante = 9999,
            IdProfesor = 1,
            IdMateria = 1,
            ValorNota = 4
        }));
    }

    [Fact]
    public async Task CrearAsync_ConProfesorInexistente_LanzaExcepcionValidacion()
    {
        var servicio = CrearServicio();

        await Assert.ThrowsAsync<ExcepcionValidacion>(() => servicio.CrearAsync(new NotaCrearDto
        {
            IdEstudiante = 1,
            IdProfesor = 9999,
            IdMateria = 1,
            ValorNota = 4
        }));
    }

    [Fact]
    public async Task CrearAsync_ConMateriaInexistente_LanzaExcepcionValidacion()
    {
        var servicio = CrearServicio();

        await Assert.ThrowsAsync<ExcepcionValidacion>(() => servicio.CrearAsync(new NotaCrearDto
        {
            IdEstudiante = 1,
            IdProfesor = 1,
            IdMateria = 9999,
            ValorNota = 4
        }));
    }

    [Fact]
    public async Task ObtenerPorIdAsync_ConIdInexistente_LanzaExcepcionNoEncontrado()
    {
        var servicio = CrearServicio();

        await Assert.ThrowsAsync<ExcepcionNoEncontrado>(() => servicio.ObtenerPorIdAsync(9999));
    }

    [Fact]
    public async Task ObtenerResumenPaginadoAsync_CalculaElPromedioPorEstudianteYMateria()
    {
        var servicio = CrearServicio();

        // Datos semilla: estudiante 1, materia 1 -> notas 4.5 y 3.8 => promedio 4.15
        var resumen = await servicio.ObtenerResumenPaginadoAsync(numeroPagina: 1, tamanoPagina: 50);

        var filaEstudiante1Materia1 = resumen.Datos.Single(r => r.IdEstudiante == 1 && r.IdMateria == 1);

        Assert.Equal(2, filaEstudiante1Materia1.CantidadNotas);
        Assert.Equal(4.15m, filaEstudiante1Materia1.PromedioNota);
    }

    [Fact]
    public async Task ObtenerPorEstudianteYMateriaAsync_RetornaSoloLasNotasDeEseEstudianteYMateria()
    {
        var servicio = CrearServicio();

        var notas = await servicio.ObtenerPorEstudianteYMateriaAsync(idEstudiante: 1, idMateria: 1);

        Assert.All(notas, n =>
        {
            Assert.Equal(1, n.IdEstudiante);
            Assert.Equal(1, n.IdMateria);
        });
        Assert.Equal(2, notas.Count());
    }

    [Fact]
    public async Task EliminarAsync_ConIdExistente_EliminaLaNota()
    {
        var servicio = CrearServicio();

        await servicio.EliminarAsync(1);

        await Assert.ThrowsAsync<ExcepcionNoEncontrado>(() => servicio.ObtenerPorIdAsync(1));
    }

    [Fact]
    public async Task EliminarAsync_ConIdInexistente_LanzaExcepcionNoEncontrado()
    {
        var servicio = CrearServicio();

        await Assert.ThrowsAsync<ExcepcionNoEncontrado>(() => servicio.EliminarAsync(9999));
    }

    [Fact]
    public async Task ActualizarAsync_ConIdDeRutaDistintoAlDelCuerpo_LanzaExcepcionValidacion()
    {
        var servicio = CrearServicio();

        await Assert.ThrowsAsync<ExcepcionValidacion>(() => servicio.ActualizarAsync(1, new NotaActualizarDto
        {
            Id = 2,
            IdEstudiante = 1,
            IdProfesor = 1,
            IdMateria = 1,
            ValorNota = 4
        }));
    }
}
