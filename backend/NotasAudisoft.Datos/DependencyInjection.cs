using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotasAudisoft.Datos.Repositorios;

namespace NotasAudisoft.Datos;

public static class DependencyInjection
{
    public static IServiceCollection AgregarCapaDatos(this IServiceCollection servicios, IConfiguration configuracion)
    {
        servicios.AddDbContext<NotasDbContext>(opciones =>
            opciones.UseSqlServer(configuracion.GetConnectionString("NotasDbConnection")));

        servicios.AddScoped<IEstudianteRepositorio, EstudianteRepositorio>();
        servicios.AddScoped<IProfesorRepositorio, ProfesorRepositorio>();
        servicios.AddScoped<IMateriaRepositorio, MateriaRepositorio>();
        servicios.AddScoped<INotaRepositorio, NotaRepositorio>();

        return servicios;
    }
}
