using System.ComponentModel.DataAnnotations;

namespace ClubIngles.Data.Models;

public enum TipoEvaluacion { Diagnostico, PresentacionOral, ReporteLibro }

// Entidad fuerte en relación 1:N con Estudiante (relación presenta)
public class Evaluacion
{
    [Key]
    public int IdEvaluacion { get; set; }

    public int IdEstudiante { get; set; }   // FK -> Estudiante
    public Estudiante? Estudiante { get; set; }

    public TipoEvaluacion TipoEvaluacion { get; set; }

    public DateTime Fecha { get; set; } = DateTime.Today;

    [Range(0, 100)]
    public decimal Calificacion { get; set; }

    [StringLength(2)]
    public string NivelCefrAsignado { get; set; } = "A1";
}
