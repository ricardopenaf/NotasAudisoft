namespace NotasAudisoft.Negocio.Dtos;

public class NotaDto
{
    public int Id { get; set; }
    public int IdEstudiante { get; set; }
    public string NombreEstudiante { get; set; } = string.Empty;
    public int IdProfesor { get; set; }
    public string NombreProfesor { get; set; } = string.Empty;
    public int IdMateria { get; set; }
    public string NombreMateria { get; set; } = string.Empty;
    public decimal ValorNota { get; set; }
}

public class NotaCrearDto
{
    public int IdEstudiante { get; set; }
    public int IdProfesor { get; set; }
    public int IdMateria { get; set; }
    public decimal ValorNota { get; set; }
}

public class NotaActualizarDto
{
    public int Id { get; set; }
    public int IdEstudiante { get; set; }
    public int IdProfesor { get; set; }
    public int IdMateria { get; set; }
    public decimal ValorNota { get; set; }
}

/// <summary>
/// Agrupa las notas de un estudiante en una materia y expone el promedio,
/// según el requerimiento de mostrar la nota como el promedio por materia.
/// </summary>
public class NotaResumenDto
{
    public int IdEstudiante { get; set; }
    public string NombreEstudiante { get; set; } = string.Empty;
    public int IdMateria { get; set; }
    public string NombreMateria { get; set; } = string.Empty;
    public int IdProfesor { get; set; }
    public string NombreProfesor { get; set; } = string.Empty;
    public decimal PromedioNota { get; set; }
    public int CantidadNotas { get; set; }
}
