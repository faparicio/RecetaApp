using Microsoft.JSInterop;
using RecetaApp.Models;
using System.Text.Json;

namespace RecetaApp.Services;

public class PresionArterialService
{
    private readonly IJSRuntime _js;
    private readonly AuthService _auth;
    private const string Collection = "presionArterial";

    public PresionArterialService(IJSRuntime js, AuthService auth)
    {
        _js = js;
        _auth = auth;
    }

    public async Task<List<PresionArterial>> GetAllAsync()
    {
        var result = await _js.InvokeAsync<JsonElement>("firestore.query", Collection, new
        {
            filters = new[] { new { field = "userId", op = "==", value = _auth.CurrentUser?.Uid } }
        });

        if (!result.GetProperty("success").GetBoolean()) return new();

        var list = new List<PresionArterial>();
        foreach (var item in result.GetProperty("data").EnumerateArray())
        {
            list.Add(MapFromJson(item));
        }
        return list.OrderByDescending(p => p.Fecha).ThenByDescending(p => p.Hora).ToList();
    }

    public async Task<List<PresionArterial>> GetByPacienteAsync(string pacienteId)
    {
        var all = await GetAllAsync();
        return all.Where(p => p.PacienteId == pacienteId).ToList();
    }

    public async Task<(bool Success, string? Error, string? Id)> AddAsync(PresionArterial entity)
    {
        entity.UserId = _auth.CurrentUser?.Uid ?? string.Empty;
        entity.CreatedAt = DateTime.UtcNow.ToString("o");

        var result = await _js.InvokeAsync<JsonElement>("firestore.add", Collection, MapToDict(entity));
        if (result.GetProperty("success").GetBoolean())
            return (true, null, result.GetProperty("id").GetString());
        return (false, result.GetProperty("error").GetString(), null);
    }

    public async Task<(bool Success, string? Error)> UpdateAsync(PresionArterial entity)
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

    private static PresionArterial MapFromJson(JsonElement el)
    {
        return new PresionArterial
        {
            Id = el.TryGetProperty("id", out var id) ? id.GetString() ?? "" : "",
            PacienteId = el.TryGetProperty("pacienteId", out var pid) ? pid.GetString() ?? "" : "",
            PacienteNombre = el.TryGetProperty("pacienteNombre", out var pn) ? pn.GetString() ?? "" : "",
            Sistolica = el.TryGetProperty("sistolica", out var s) ? s.TryGetInt32(out var sv) ? sv : 0 : 0,
            Diastolica = el.TryGetProperty("diastolica", out var d) ? d.TryGetInt32(out var dv) ? dv : 0 : 0,
            Pulso = el.TryGetProperty("pulso", out var p) ? p.TryGetInt32(out var pv) ? pv : 0 : 0,
            Fecha = el.TryGetProperty("fecha", out var f) ? f.GetString() ?? "" : "",
            Hora = el.TryGetProperty("hora", out var h) ? h.GetString() ?? "" : "",
            Notas = el.TryGetProperty("notas", out var n) ? n.GetString() ?? "" : "",
            UserId = el.TryGetProperty("userId", out var u) ? u.GetString() ?? "" : "",
            CreatedAt = el.TryGetProperty("createdAt", out var c) ? c.GetString() ?? "" : ""
        };
    }

    private static Dictionary<string, object> MapToDict(PresionArterial p)
    {
        return new Dictionary<string, object>
        {
            ["pacienteId"] = p.PacienteId,
            ["pacienteNombre"] = p.PacienteNombre,
            ["sistolica"] = p.Sistolica,
            ["diastolica"] = p.Diastolica,
            ["pulso"] = p.Pulso,
            ["fecha"] = p.Fecha,
            ["hora"] = p.Hora,
            ["notas"] = p.Notas,
            ["userId"] = p.UserId,
            ["createdAt"] = p.CreatedAt
        };
    }
}
