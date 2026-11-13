using Microsoft.Extensions.DependencyInjection;
using NotasAudisoft.Negocio.Servicios;

namespace NotasAudisoft.Negocio;

public static class DependencyInjection
{
    public static IServiceCollection AgregarCapaNegocio(this IServiceCollection servicios)
    {
        servicios.AddScoped<IEstudianteServicio, EstudianteServicio>();
        servicios.AddScoped<IProfesorServicio, ProfesorServicio>();
        servicios.AddScoped<IMateriaServicio, MateriaServicio>();
        servicios.AddScoped<INotaServicio, NotaServicio>();

        return servicios;
    }
}
