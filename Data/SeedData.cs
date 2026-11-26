using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ClubIngles.Data.Models;

namespace ClubIngles.Data;

// Datos de prueba: solo se agregan si la tabla Estudiante está vacía.
public static class SeedData
{
    public static async Task CargarAsync(IServiceProvider services)
    {
        var factory = services.GetRequiredService<IDbContextFactory<ApplicationDbContext>>();
        using var db = factory.CreateDbContext();

        if (await db.Estudiantes.AnyAsync()) return;

        // ---------- Estudiantes ----------
        var estudiantes = new List<Estudiante>
        {
            new() { Matricula = "20240101", NombreCompleto = "Ana Sofía Martínez López",   Correo = "amartinez@ucol.mx",  NivelCefrActual = "A2" },
            new() { Matricula = "20240102", NombreCompleto = "Luis Fernando García Ruiz",  Correo = "lgarcia@ucol.mx",    NivelCefrActual = "B1" },
            new() { Matricula = "20240103", NombreCompleto = "María José Hernández Cruz",  Correo = "mhernandez@ucol.mx", NivelCefrActual = "A2" },
            new() { Matricula = "20240104", NombreCompleto = "Carlos Eduardo Ramírez Torres", Correo = "cramirez@ucol.mx", NivelCefrActual = "B1" },
            new() { Matricula = "20240105", NombreCompleto = "Valeria Gómez Sánchez",      Correo = "vgomez@ucol.mx",     NivelCefrActual = "A2" },
            new() { Matricula = "20240106", NombreCompleto = "Diego Alejandro Flores Díaz", Correo = "dflores@ucol.mx",   NivelCefrActual = "B2" },
            new() { Matricula = "20240107", NombreCompleto = "Fernanda Ortiz Mendoza",     Correo = "fortiz@ucol.mx",     NivelCefrActual = "A1" },
            new() { Matricula = "20240108", NombreCompleto = "Jorge Iván Castillo Vargas", Correo = "jcastillo@ucol.mx",  NivelCefrActual = "B1" },
            new() { Matricula = "20240109", NombreCompleto = "Daniela Morales Reyes",      Correo = "dmorales@ucol.mx",   NivelCefrActual = "A2" },
            new() { Matricula = "20240110", NombreCompleto = "Ricardo Jiménez Aguilar",    Correo = "rjimenez@ucol.mx",   NivelCefrActual = "B1" },
        };
        foreach (var e in estudiantes) e.FechaInscripcion = new DateTime(2026, 8, 31);
        db.Estudiantes.AddRange(estudiantes);

        // ---------- Sesiones: 12 jueves ----------
        string[] temas =
        {
            "Examen diagnóstico y presentación del club",
            "Introducing yourself",
            "Daily routines",
            "Talking about the past",
            "Food and restaurants",
            "Travel and directions",
            "Movies and series",
            "Future plans",
            "Technology in our lives",
            "Presentaciones orales",
            "Debate: social media",
            "Reporte de libro y cierre",
        };
        var sesiones = new List<Sesion>();
        var primerJueves = new DateTime(2026, 9, 3);
        for (int i = 0; i < 12; i++)
            sesiones.Add(new Sesion { Semana = i + 1, Fecha = primerJueves.AddDays(7 * i), Tema = temas[i] });
        db.Sesiones.AddRange(sesiones);

        // ---------- Asistencia (relación N:M) ----------
        var rnd = new Random(42); // semilla fija = mismos datos siempre
        foreach (var e in estudiantes)
            foreach (var s in sesiones)
            {
                int r = rnd.Next(100);
                var estado = r < 78 ? EstadoAsistencia.Presente
                           : r < 90 ? EstadoAsistencia.Retardo
                           : EstadoAsistencia.Ausente;
                db.Asistencias.Add(new Asistencia { Estudiante = e, Sesion = s, Estado = estado });
            }

        // ---------- Evaluaciones (relación 1:N) ----------
        foreach (var e in estudiantes)
        {
            db.Evaluaciones.Add(new Evaluacion { Estudiante = e, TipoEvaluacion = TipoEvaluacion.Diagnostico,
                Fecha = sesiones[0].Fecha, Calificacion = rnd.Next(55, 90), NivelCefrAsignado = e.NivelCefrActual });
            db.Evaluaciones.Add(new Evaluacion { Estudiante = e, TipoEvaluacion = TipoEvaluacion.PresentacionOral,
                Fecha = sesiones[9].Fecha, Calificacion = rnd.Next(65, 100), NivelCefrAsignado = e.NivelCefrActual });
            db.Evaluaciones.Add(new Evaluacion { Estudiante = e, TipoEvaluacion = TipoEvaluacion.ReporteLibro,
                Fecha = sesiones[11].Fecha, Calificacion = rnd.Next(70, 100), NivelCefrAsignado = e.NivelCefrActual });
        }

        await db.SaveChangesAsync();
    }

    // Crea los roles y vuelve Coordinador a cualquier cuenta que no tenga rol
    // (por ejemplo, la cuenta que se registró antes de que existieran los roles).
    public static async Task RolesAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var rol in new[] { Roles.Coordinador, Roles.Estudiante })
            if (!await roleManager.RoleExistsAsync(rol))
                await roleManager.CreateAsync(new IdentityRole(rol));

        foreach (var user in await userManager.Users.ToListAsync())
            if ((await userManager.GetRolesAsync(user)).Count == 0)
                await userManager.AddToRoleAsync(user, Roles.Coordinador);
    }
}
