namespace NotasAudisoft.Negocio.Dtos;

public class EstudianteDto
{
    public int Id { get; set; }
    public string NombreEstudiante { get; set; } = string.Empty;
    public long NumeroIdentificacion { get; set; }
}

public class EstudianteCrearDto
{
    public string NombreEstudiante { get; set; } = string.Empty;
    public long NumeroIdentificacion { get; set; }
}

public class EstudianteActualizarDto
{
    public int Id { get; set; }
    public string NombreEstudiante { get; set; } = string.Empty;
    public long NumeroIdentificacion { get; set; }
}
