namespace NotasAudisoft.Negocio.Dtos;

public class MateriaDto
{
    public int Id { get; set; }
    public string NombreMateria { get; set; } = string.Empty;
}

public class MateriaCrearDto
{
    public string NombreMateria { get; set; } = string.Empty;
}

public class MateriaActualizarDto
{
    public int Id { get; set; }
    public string NombreMateria { get; set; } = string.Empty;
}
