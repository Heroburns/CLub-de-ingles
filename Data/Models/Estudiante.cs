using System.ComponentModel.DataAnnotations;

namespace ClubIngles.Data.Models;

// Entidad fuerte: cada participante del club
public class Estudiante
{
    [Key]
    public int IdEstudiante { get; set; }

    [Required, StringLength(20)]
    public string Matricula { get; set; } = "";

    [Required, StringLength(100)]
    public string NombreCompleto { get; set; } = "";

    [Required, EmailAddress, StringLength(100)]
    public string Correo { get; set; } = "";

    [StringLength(2)]
    public string NivelCefrActual { get; set; } = "A1"; // A1, A2, B1, B2, C1, C2

    public DateTime FechaInscripcion { get; set; } = DateTime.Today;

    // Relación asiste_a (N:M) vía Asistencia
    public List<Asistencia> Asistencias { get; set; } = new();

    // Relación presenta (1:N) con Evaluacion
    public List<Evaluacion> Evaluaciones { get; set; } = new();
}
