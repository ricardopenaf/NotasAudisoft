namespace NotasAudisoft.Entidades;

public class Estudiante
{
    public int Id { get; set; }
    public string NombreEstudiante { get; set; } = string.Empty;
    public long NumeroIdentificacion { get; set; }

    public ICollection<Nota> Notas { get; set; } = new List<Nota>();
}
