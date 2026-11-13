namespace NotasAudisoft.Entidades;

public class Materia
{
    public int Id { get; set; }
    public string NombreMateria { get; set; } = string.Empty;

    public ICollection<Nota> Notas { get; set; } = new List<Nota>();
}
