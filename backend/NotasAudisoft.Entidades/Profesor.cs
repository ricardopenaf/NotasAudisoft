namespace NotasAudisoft.Entidades;

public class Profesor
{
    public int Id { get; set; }
    public string NombreProfesor { get; set; } = string.Empty;
    public long NumeroIdentificacion { get; set; }

    public ICollection<Nota> Notas { get; set; } = new List<Nota>();
}
