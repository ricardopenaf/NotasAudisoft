using Microsoft.EntityFrameworkCore;
using NotasAudisoft.Entidades;

namespace NotasAudisoft.Datos;

public class NotasDbContext : DbContext
{
    public NotasDbContext(DbContextOptions<NotasDbContext> options) : base(options)
    {
    }

    public DbSet<Estudiante> Estudiantes => Set<Estudiante>();
    public DbSet<Profesor> Profesores => Set<Profesor>();
    public DbSet<Materia> Materias => Set<Materia>();
    public DbSet<Nota> Notas => Set<Nota>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Estudiante>(entity =>
        {
            entity.ToTable("Estudiante");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NombreEstudiante)
                .IsRequired()
                .HasMaxLength(150);
            entity.Property(e => e.NumeroIdentificacion)
                .IsRequired();
            entity.HasIndex(e => e.NumeroIdentificacion)
                .IsUnique();
        });

        modelBuilder.Entity<Profesor>(entity =>
        {
            entity.ToTable("Profesor");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NombreProfesor)
                .IsRequired()
                .HasMaxLength(150);
            entity.Property(e => e.NumeroIdentificacion)
                .IsRequired();
            entity.HasIndex(e => e.NumeroIdentificacion)
                .IsUnique();
        });

        modelBuilder.Entity<Materia>(entity =>
        {
            entity.ToTable("Materia");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NombreMateria)
                .IsRequired()
                .HasMaxLength(150);
        });

        modelBuilder.Entity<Nota>(entity =>
        {
            entity.ToTable("Nota");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ValorNota)
                .IsRequired()
                .HasColumnType("decimal(3,1)");

            entity.HasOne(n => n.Estudiante)
                .WithMany(e => e.Notas)
                .HasForeignKey(n => n.IdEstudiante)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(n => n.Profesor)
                .WithMany(p => p.Notas)
                .HasForeignKey(n => n.IdProfesor)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(n => n.Materia)
                .WithMany(m => m.Notas)
                .HasForeignKey(n => n.IdMateria)
                .OnDelete(DeleteBehavior.Restrict);

            entity.ToTable(t => t.HasCheckConstraint("CK_Nota_ValorNota_Rango", "[ValorNota] >= 0 AND [ValorNota] <= 5"));
        });

        SembrarDatos(modelBuilder);
    }

    private static void SembrarDatos(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Estudiante>().HasData(
            new Estudiante { Id = 1, NombreEstudiante = "Ana María Gómez", NumeroIdentificacion = 1001 },
            new Estudiante { Id = 2, NombreEstudiante = "Carlos Pérez", NumeroIdentificacion = 1002 },
            new Estudiante { Id = 3, NombreEstudiante = "Luisa Fernández", NumeroIdentificacion = 1003 },
            new Estudiante { Id = 4, NombreEstudiante = "Juan Rodríguez", NumeroIdentificacion = 1004 }
        );

        modelBuilder.Entity<Profesor>().HasData(
            new Profesor { Id = 1, NombreProfesor = "Jorge Ramírez", NumeroIdentificacion = 2001 },
            new Profesor { Id = 2, NombreProfesor = "Patricia Londoño", NumeroIdentificacion = 2002 },
            new Profesor { Id = 3, NombreProfesor = "Andrés Torres", NumeroIdentificacion = 2003 }
        );

        modelBuilder.Entity<Materia>().HasData(
            new Materia { Id = 1, NombreMateria = "Matemáticas" },
            new Materia { Id = 2, NombreMateria = "Lengua Castellana" },
            new Materia { Id = 3, NombreMateria = "Ciencias Naturales" },
            new Materia { Id = 4, NombreMateria = "Inglés" }
        );

        modelBuilder.Entity<Nota>().HasData(
            new Nota { Id = 1, IdEstudiante = 1, IdProfesor = 1, IdMateria = 1, ValorNota = 4.5m },
            new Nota { Id = 2, IdEstudiante = 1, IdProfesor = 1, IdMateria = 1, ValorNota = 3.8m },
            new Nota { Id = 3, IdEstudiante = 1, IdProfesor = 2, IdMateria = 2, ValorNota = 4.0m },
            new Nota { Id = 4, IdEstudiante = 2, IdProfesor = 1, IdMateria = 1, ValorNota = 2.5m },
            new Nota { Id = 5, IdEstudiante = 2, IdProfesor = 3, IdMateria = 3, ValorNota = 3.2m },
            new Nota { Id = 6, IdEstudiante = 3, IdProfesor = 2, IdMateria = 2, ValorNota = 5.0m },
            new Nota { Id = 7, IdEstudiante = 3, IdProfesor = 3, IdMateria = 4, ValorNota = 4.2m },
            new Nota { Id = 8, IdEstudiante = 4, IdProfesor = 1, IdMateria = 1, ValorNota = 3.0m },
            new Nota { Id = 9, IdEstudiante = 4, IdProfesor = 3, IdMateria = 4, ValorNota = 4.8m },
            new Nota { Id = 10, IdEstudiante = 2, IdProfesor = 1, IdMateria = 1, ValorNota = 3.9m }
        );
    }
}
