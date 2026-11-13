namespace NotasAudisoft.Negocio.Dtos;

public class ProfesorDto
{
    public int Id { get; set; }
    public string NombreProfesor { get; set; } = string.Empty;
    public long NumeroIdentificacion { get; set; }
}

public class ProfesorCrearDto
{
    public string NombreProfesor { get; set; } = string.Empty;
    public long NumeroIdentificacion { get; set; }
}

public class ProfesorActualizarDto
{
    public int Id { get; set; }
    public string NombreProfesor { get; set; } = string.Empty;
    public long NumeroIdentificacion { get; set; }
}
