namespace NotasAudisoft.Negocio.Excepciones;

public class ExcepcionNegocio : Exception
{
    public ExcepcionNegocio(string mensaje) : base(mensaje)
    {
    }
}

public class ExcepcionNoEncontrado : Exception
{
    public ExcepcionNoEncontrado(string mensaje) : base(mensaje)
    {
    }
}

public class ExcepcionValidacion : Exception
{
    public ExcepcionValidacion(string mensaje) : base(mensaje)
    {
    }
}
