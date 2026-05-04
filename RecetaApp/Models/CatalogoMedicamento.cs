namespace RecetaApp.Models;

public class CatalogoMedicamento
{
    public string Id { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string SustanciaActiva { get; set; } = string.Empty;
    public string Presentacion { get; set; } = string.Empty;
    public string Instrucciones { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string CreatedAt { get; set; } = string.Empty;
}
