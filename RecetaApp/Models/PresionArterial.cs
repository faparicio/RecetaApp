namespace RecetaApp.Models;

public class PresionArterial
{
    public string Id { get; set; } = string.Empty;
    public string PacienteId { get; set; } = string.Empty;
    public string PacienteNombre { get; set; } = string.Empty;
    public int Sistolica { get; set; }
    public int Diastolica { get; set; }
    public int Pulso { get; set; }
    public string Fecha { get; set; } = string.Empty;
    public string Hora { get; set; } = string.Empty;
    public string Notas { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string CreatedAt { get; set; } = string.Empty;

    public string Clasificacion
    {
        get
        {
            if (Sistolica >= 140 || Diastolica >= 90) return "Hipertensión Etapa 2";
            if (Sistolica >= 130 || Diastolica >= 80) return "Hipertensión Etapa 1";
            if (Sistolica >= 120) return "Elevada";
            if (Sistolica > 0 && Diastolica > 0) return "Normal";
            return "Sin clasificar";
        }
    }

    public string ClasificacionColor => Clasificacion switch
    {
        "Normal" => "success",
        "Elevada" => "warning",
        "Hipertensión Etapa 1" => "orange",
        "Hipertensión Etapa 2" => "danger",
        _ => "secondary"
    };
}
