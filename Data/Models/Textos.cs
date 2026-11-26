namespace ClubIngles.Data.Models;

// Textos bonitos para mostrar en pantalla
public static class Textos
{
    public static string Tipo(TipoEvaluacion t) => t switch
    {
        TipoEvaluacion.Diagnostico => "Examen diagnóstico",
        TipoEvaluacion.PresentacionOral => "Presentación oral",
        TipoEvaluacion.ReporteLibro => "Reporte de libro",
        _ => t.ToString()
    };

    public static string ColorEstado(EstadoAsistencia e) => e switch
    {
        EstadoAsistencia.Presente => "bg-success",
        EstadoAsistencia.Retardo => "bg-warning text-dark",
        _ => "bg-danger"
    };

    public static readonly string[] Niveles = { "A1", "A2", "B1", "B2", "C1", "C2" };
}
