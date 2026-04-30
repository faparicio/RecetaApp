namespace RecetaApp.Models;

public class Medicamento
{
    public string Id { get; set; } = string.Empty;
    public string RecetaId { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Dosis { get; set; } = string.Empty;
    public string Frecuencia { get; set; } = string.Empty;
    public string Duracion { get; set; } = string.Empty;
    public string Instrucciones { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
}
