using Microsoft.JSInterop;
using RecetaApp.Models;
using System.Text.Json;

namespace RecetaApp.Services;

public class MedicamentoService
{
    private readonly IJSRuntime _js;
    private readonly AuthService _auth;
    private const string Collection = "medicamentos";

    public MedicamentoService(IJSRuntime js, AuthService auth)
    {
        _js = js;
        _auth = auth;
    }

    public async Task<List<Medicamento>> GetByRecetaAsync(string recetaId)
    {
        var result = await _js.InvokeAsync<JsonElement>("firestore.query", Collection, new
        {
            filters = new object[]
            {
                new { field = "userId", op = "==", value = _auth.CurrentUser?.Uid },
                new { field = "recetaId", op = "==", value = recetaId }
            }
        });

        if (!result.GetProperty("success").GetBoolean()) return new();

        var list = new List<Medicamento>();
        foreach (var item in result.GetProperty("data").EnumerateArray())
        {
            list.Add(MapFromJson(item));
        }
        return list.OrderBy(m => m.Nombre).ToList();
    }

    public async Task<(bool Success, string? Error, string? Id)> AddAsync(Medicamento entity)
    {
        entity.UserId = _auth.CurrentUser?.Uid ?? string.Empty;

        var result = await _js.InvokeAsync<JsonElement>("firestore.add", Collection, MapToDict(entity));
        if (result.GetProperty("success").GetBoolean())
            return (true, null, result.GetProperty("id").GetString());
        return (false, result.GetProperty("error").GetString(), null);
    }

    public async Task<(bool Success, string? Error)> UpdateAsync(Medicamento entity)
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

    public async Task DeleteByRecetaAsync(string recetaId)
    {
        var medicamentos = await GetByRecetaAsync(recetaId);
        foreach (var med in medicamentos)
        {
            await DeleteAsync(med.Id);
        }
    }

    private static Medicamento MapFromJson(JsonElement el)
    {
        return new Medicamento
        {
            Id = el.TryGetProperty("id", out var id) ? id.GetString() ?? "" : "",
            RecetaId = el.TryGetProperty("recetaId", out var ri) ? ri.GetString() ?? "" : "",
            Nombre = el.TryGetProperty("nombre", out var n) ? n.GetString() ?? "" : "",
            Dosis = el.TryGetProperty("dosis", out var d) ? d.GetString() ?? "" : "",
            Frecuencia = el.TryGetProperty("frecuencia", out var f) ? f.GetString() ?? "" : "",
            Duracion = el.TryGetProperty("duracion", out var du) ? du.GetString() ?? "" : "",
            Instrucciones = el.TryGetProperty("instrucciones", out var i) ? i.GetString() ?? "" : "",
            UserId = el.TryGetProperty("userId", out var u) ? u.GetString() ?? "" : ""
        };
    }

    private static Dictionary<string, object> MapToDict(Medicamento m)
    {
        return new Dictionary<string, object>
        {
            ["recetaId"] = m.RecetaId,
            ["nombre"] = m.Nombre,
            ["dosis"] = m.Dosis,
            ["frecuencia"] = m.Frecuencia,
            ["duracion"] = m.Duracion,
            ["instrucciones"] = m.Instrucciones,
            ["userId"] = m.UserId
        };
    }
}
