namespace NotasAudisoft.Entidades.Comun;

public class ResultadoPaginado<T>
{
    public IEnumerable<T> Datos { get; set; } = Array.Empty<T>();
    public int PaginaActual { get; set; }
    public int TamanoPagina { get; set; }
    public int TotalRegistros { get; set; }
    public int TotalPaginas => TamanoPagina == 0 ? 0 : (int)Math.Ceiling(TotalRegistros / (double)TamanoPagina);
}
