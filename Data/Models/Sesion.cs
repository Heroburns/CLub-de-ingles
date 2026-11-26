using System.ComponentModel.DataAnnotations;

namespace ClubIngles.Data.Models;

// Entidad fuerte: cada encuentro semanal del club
public class Sesion
{
    [Key]
    public int IdSesion { get; set; }

    public DateTime Fecha { get; set; } = DateTime.Today;

    [Range(1, 12)]
    public int Semana { get; set; } = 1;

    [Required, StringLength(150)]
    public string Tema { get; set; } = "";

    // Relación asiste_a (N:M) vía Asistencia
    public List<Asistencia> Asistencias { get; set; } = new();
}
