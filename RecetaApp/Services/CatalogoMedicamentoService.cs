using Microsoft.JSInterop;
using RecetaApp.Models;
using System.Text.Json;

namespace RecetaApp.Services;

public class CatalogoMedicamentoService
{
    private readonly IJSRuntime _js;
    private readonly AuthService _auth;
    private const string Collection = "catalogoMedicamentos";

    public CatalogoMedicamentoService(IJSRuntime js, AuthService auth)
    {
        _js = js;
        _auth = auth;
    }

    public async Task<List<CatalogoMedicamento>> GetAllAsync()
    {
        var result = await _js.InvokeAsync<JsonElement>("firestore.query", Collection, new
        {
            filters = new[] { new { field = "userId", op = "==", value = _auth.CurrentUser?.Uid } }
        });

        if (!result.GetProperty("success").GetBoolean()) return new();

        var list = new List<CatalogoMedicamento>();
        foreach (var item in result.GetProperty("data").EnumerateArray())
        {
            list.Add(MapFromJson(item));
        }
        return list.OrderBy(m => m.Nombre).ToList();
    }

    public async Task<(bool Success, string? Error, string? Id)> AddAsync(CatalogoMedicamento entity)
    {
        entity.UserId = _auth.CurrentUser?.Uid ?? string.Empty;
        entity.CreatedAt = DateTime.UtcNow.ToString("o");

        var result = await _js.InvokeAsync<JsonElement>("firestore.add", Collection, MapToDict(entity));
        if (result.GetProperty("success").GetBoolean())
            return (true, null, result.GetProperty("id").GetString());
        return (false, result.GetProperty("error").GetString(), null);
    }

    public async Task<(bool Success, string? Error)> UpdateAsync(CatalogoMedicamento entity)
    {
        var result = await _js.InvokeAsync<JsonElement>("firestore.update", Collection, entity.Id, MapToDict(entity));
        if (result.GetProperty("success").GetBoolean())
            return (true, null);
        return (false, result.GetProperty("error").GetString());
    }

    public async Task<(bool Success, string? Error)> DeleteAsync(string id)
    {
        var result = await _js.InvokeAsync<JsonElement>("firestore.delete", Collection, id);
        if (result.GetProperty("success").GetBoolean())
            return (true, null);
        return (false, result.GetProperty("error").GetString());
    }

    private static CatalogoMedicamento MapFromJson(JsonElement el)
    {
        return new CatalogoMedicamento
        {
            Id = el.TryGetProperty("id", out var id) ? id.GetString() ?? "" : "",
            Nombre = el.TryGetProperty("nombre", out var n) ? n.GetString() ?? "" : "",
            SustanciaActiva = el.TryGetProperty("sustanciaActiva", out var sa) ? sa.GetString() ?? "" : "",
            Presentacion = el.TryGetProperty("presentacion", out var pr) ? pr.GetString() ?? "" : "",
            Instrucciones = el.TryGetProperty("instrucciones", out var i) ? i.GetString() ?? "" : "",
            UserId = el.TryGetProperty("userId", out var u) ? u.GetString() ?? "" : "",
            CreatedAt = el.TryGetProperty("createdAt", out var c) ? c.GetString() ?? "" : ""
        };
    }

    private static Dictionary<string, object> MapToDict(CatalogoMedicamento m)
    {
        return new Dictionary<string, object>
        {
            ["nombre"] = m.Nombre,
            ["sustanciaActiva"] = m.SustanciaActiva,
            ["presentacion"] = m.Presentacion,
            ["instrucciones"] = m.Instrucciones,
            ["userId"] = m.UserId,
            ["createdAt"] = m.CreatedAt
        };
    }
}
