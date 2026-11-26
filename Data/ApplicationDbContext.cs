using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ClubIngles.Data.Models;

namespace ClubIngles.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Estudiante> Estudiantes => Set<Estudiante>();
    public DbSet<Sesion> Sesiones => Set<Sesion>();
    public DbSet<Asistencia> Asistencias => Set<Asistencia>();
    public DbSet<Evaluacion> Evaluaciones => Set<Evaluacion>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); // tablas del login (Identity)

        builder.Entity<Estudiante>().ToTable("Estudiante");
        builder.Entity<Estudiante>().HasIndex(e => e.Matricula).IsUnique();

        builder.Entity<Sesion>().ToTable("Sesion");

        // Asistencia: un estudiante solo tiene un registro por sesión
        builder.Entity<Asistencia>().ToTable("Asistencia");
        builder.Entity<Asistencia>()
            .HasOne(a => a.Estudiante).WithMany(e => e.Asistencias)
            .HasForeignKey(a => a.IdEstudiante).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Asistencia>()
            .HasOne(a => a.Sesion).WithMany(s => s.Asistencias)
            .HasForeignKey(a => a.IdSesion).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Asistencia>()
            .HasIndex(a => new { a.IdEstudiante, a.IdSesion }).IsUnique();
        builder.Entity<Asistencia>().Property(a => a.Estado).HasConversion<string>().HasMaxLength(10);

        builder.Entity<Evaluacion>().ToTable("Evaluacion");
        builder.Entity<Evaluacion>()
            .HasOne(v => v.Estudiante).WithMany(e => e.Evaluaciones)
            .HasForeignKey(v => v.IdEstudiante).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Evaluacion>().Property(v => v.TipoEvaluacion).HasConversion<string>().HasMaxLength(20);
        builder.Entity<Evaluacion>().Property(v => v.Calificacion).HasPrecision(5, 2);
    }
}
