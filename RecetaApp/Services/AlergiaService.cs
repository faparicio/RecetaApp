using Microsoft.JSInterop;
using RecetaApp.Models;
using System.Text.Json;

namespace RecetaApp.Services;

public class AlergiaService
{
    private readonly IJSRuntime _js;
    private readonly AuthService _auth;
    private const string Collection = "alergias";

    public AlergiaService(IJSRuntime js, AuthService auth)
    {
        _js = js;
        _auth = auth;
    }

    public async Task<List<Alergia>> GetAllAsync()
    {
        var result = await _js.InvokeAsync<JsonElement>("firestore.query", Collection, new
        {
            filters = new[] { new { field = "userId", op = "==", value = _auth.CurrentUser?.Uid } }
        });

        if (!result.GetProperty("success").GetBoolean()) return new();

        var list = new List<Alergia>();
        foreach (var item in result.GetProperty("data").EnumerateArray())
        {
            list.Add(MapFromJson(item));
        }
        return list.OrderByDescending(a => a.SeveridadPeso).ThenBy(a => a.Nombre).ToList();
    }

    public async Task<List<Alergia>> GetByPacienteAsync(string pacienteId)
    {
        var all = await GetAllAsync();
        return all.Where(a => a.PacienteId == pacienteId).ToList();
    }

    public async Task<(bool Success, string? Error, string? Id)> AddAsync(Alergia entity)
    {
        entity.UserId = _auth.CurrentUser?.Uid ?? string.Empty;
        entity.CreatedAt = DateTime.UtcNow.ToString("o");

        var result = await _js.InvokeAsync<JsonElement>("firestore.add", Collection, MapToDict(entity));
        if (result.GetProperty("success").GetBoolean())
            return (true, null, result.GetProperty("id").GetString());
        return (false, result.GetProperty("error").GetString(), null);
    }

    public async Task<(bool Success, string? Error)> UpdateAsync(Alergia entity)
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

    private static Alergia MapFromJson(JsonElement el)
    {
        return new Alergia
        {
            Id = el.TryGetProperty("id", out var id) ? id.GetString() ?? "" : "",
            PacienteId = el.TryGetProperty("pacienteId", out var pid) ? pid.GetString() ?? "" : "",
            PacienteNombre = el.TryGetProperty("pacienteNombre", out var pn) ? pn.GetString() ?? "" : "",
            Nombre = el.TryGetProperty("nombre", out var n) ? n.GetString() ?? "" : "",
            Severidad = el.TryGetProperty("severidad", out var s) ? s.GetString() ?? "" : "",
            Reaccion = el.TryGetProperty("reaccion", out var r) ? r.GetString() ?? "" : "",
            Notas = el.TryGetProperty("notas", out var nt) ? nt.GetString() ?? "" : "",
            UserId = el.TryGetProperty("userId", out var u) ? u.GetString() ?? "" : "",
            CreatedAt = el.TryGetProperty("createdAt", out var c) ? c.GetString() ?? "" : ""
        };
    }

    private static Dictionary<string, object> MapToDict(Alergia a)
    {
        return new Dictionary<string, object>
        {
            ["pacienteId"] = a.PacienteId,
            ["pacienteNombre"] = a.PacienteNombre,
            ["nombre"] = a.Nombre,
            ["severidad"] = a.Severidad,
            ["reaccion"] = a.Reaccion,
            ["notas"] = a.Notas,
            ["userId"] = a.UserId,
            ["createdAt"] = a.CreatedAt
        };
    }
}
