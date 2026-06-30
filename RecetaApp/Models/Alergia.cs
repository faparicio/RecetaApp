namespace RecetaApp.Models;

public class Alergia
{
    public string Id { get; set; } = string.Empty;
    public string PacienteId { get; set; } = string.Empty;
    public string PacienteNombre { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Severidad { get; set; } = string.Empty;
    public string Reaccion { get; set; } = string.Empty;
    public string Notas { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string CreatedAt { get; set; } = string.Empty;

    public string SeveridadColor => Severidad switch
    {
        "Leve" => "success",
        "Moderada" => "warning",
        "Severa" => "danger",
        _ => "secondary"
    };

    // Peso para ordenar de mayor a menor severidad
    public int SeveridadPeso => Severidad switch
    {
        "Severa" => 3,
        "Moderada" => 2,
        "Leve" => 1,
        _ => 0
    };
}
