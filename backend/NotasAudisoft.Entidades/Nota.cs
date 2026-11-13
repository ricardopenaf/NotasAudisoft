namespace NotasAudisoft.Entidades;

public class Nota
{
    public int Id { get; set; }
    public int IdMateria { get; set; }
    public int IdProfesor { get; set; }
    public int IdEstudiante { get; set; }
    public decimal ValorNota { get; set; }

    public Materia? Materia { get; set; }
    public Profesor? Profesor { get; set; }
    public Estudiante? Estudiante { get; set; }
}
