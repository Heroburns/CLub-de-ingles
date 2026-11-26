using System.ComponentModel.DataAnnotations;

namespace ClubIngles.Data.Models;

public enum EstadoAsistencia { Presente, Ausente, Retardo }

// Entidad asociativa: materializa la relación N:M entre Estudiante y Sesion
public class Asistencia
{
    [Key]
    public int IdAsistencia { get; set; }

    public int IdEstudiante { get; set; }   // FK -> Estudiante
    public Estudiante? Estudiante { get; set; }

    public int IdSesion { get; set; }       // FK -> Sesion
    public Sesion? Sesion { get; set; }

    public EstadoAsistencia Estado { get; set; } = EstadoAsistencia.Presente;
}
