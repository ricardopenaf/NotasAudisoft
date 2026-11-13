using NotasAudisoft.Negocio.Validaciones;
using Xunit;

namespace NotasAudisoft.Tests.Negocio;

public class ValidadorTextoPruebas
{
    [Theory]
    [InlineData("Ana María")]
    [InlineData("José Ángel Núñez")]
    [InlineData("Peña")]
    [InlineData("Ñoño")]
    public void EsNombreValido_ConLetrasTildesYEspacios_RetornaVerdadero(string nombre)
    {
        Assert.True(ValidadorTexto.EsNombreValido(nombre));
    }

    [Theory]
    [InlineData("Ana123")]
    [InlineData("Ana_María")]
    [InlineData("Ana@")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    [InlineData("A")]
    [InlineData("Ana  María")]
    public void EsNombreValido_ConNumerosOCaracteresEspeciales_RetornaFalso(string? nombre)
    {
        Assert.False(ValidadorTexto.EsNombreValido(nombre));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2.5)]
    [InlineData(5)]
    public void EsNotaValida_DentroDelRango_RetornaVerdadero(decimal valor)
    {
        Assert.True(ValidadorTexto.EsNotaValida(valor));
    }

    [Theory]
    [InlineData(-0.01)]
    [InlineData(5.01)]
    public void EsNotaValida_FueraDelRango_RetornaFalso(decimal valor)
    {
        Assert.False(ValidadorTexto.EsNotaValida(valor));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(1001)]
    [InlineData(9_999_999_999_999)]
    public void EsNumeroIdentificacionValido_ConValorPositivo_RetornaVerdadero(long valor)
    {
        Assert.True(ValidadorTexto.EsNumeroIdentificacionValido(valor));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void EsNumeroIdentificacionValido_ConValorNoPositivo_RetornaFalso(long valor)
    {
        Assert.False(ValidadorTexto.EsNumeroIdentificacionValido(valor));
    }
}
