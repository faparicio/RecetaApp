namespace RecetaApp.Models;

public class Receta
{
    public string Id { get; set; } = string.Empty;
    public string PacienteId { get; set; } = string.Empty;
    public string PacienteNombre { get; set; } = string.Empty;
    public string MedicoId { get; set; } = string.Empty;
    public string MedicoNombre { get; set; } = string.Empty;
    public string Fecha { get; set; } = string.Empty;
    public string Diagnostico { get; set; } = string.Empty;
    public string Notas { get; set; } = string.Empty;
    public string FotoUrl { get; set; } = string.Empty;
    public string FotoPath { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string CreatedAt { get; set; } = string.Empty;
}
