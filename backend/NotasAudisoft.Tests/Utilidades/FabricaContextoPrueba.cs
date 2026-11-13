using Microsoft.EntityFrameworkCore;
using NotasAudisoft.Datos;

namespace NotasAudisoft.Tests.Utilidades;

public static class FabricaContextoPrueba
{
    public static NotasDbContext CrearContexto()
    {
        var opciones = new DbContextOptionsBuilder<NotasDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var contexto = new NotasDbContext(opciones);
        contexto.Database.EnsureCreated();
        return contexto;
    }
}
