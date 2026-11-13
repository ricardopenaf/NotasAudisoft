using System.Text.RegularExpressions;

namespace NotasAudisoft.Negocio.Validaciones;

public static class ValidadorTexto
{
    private static readonly Regex PatronSoloLetras = new(
        @"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ]+(?:\s[A-Za-zÁÉÍÓÚÜÑáéíóúüñ]+)*$",
        RegexOptions.Compiled);

    public static bool EsNombreValido(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
        {
            return false;
        }

        var limpio = valor.Trim();
        return limpio.Length is >= 2 and <= 150 && PatronSoloLetras.IsMatch(limpio);
    }

    public static bool EsNotaValida(decimal valor)
    {
        return valor is >= 0 and <= 5;
    }

    public static bool EsNumeroIdentificacionValido(long valor)
    {
        return valor is > 0 and <= 9_999_999_999_999L;
    }
}
