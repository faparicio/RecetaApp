namespace RecetaApp.Models;

public class Paciente
{
    public string Id { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string FechaNacimiento { get; set; } = string.Empty;
    public string Alergias { get; set; } = string.Empty;
    public string Notas { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string CreatedAt { get; set; } = string.Empty;
}
