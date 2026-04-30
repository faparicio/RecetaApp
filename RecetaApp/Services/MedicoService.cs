using Microsoft.JSInterop;
using RecetaApp.Models;
using System.Text.Json;

namespace RecetaApp.Services;

public class MedicoService
{
    private readonly IJSRuntime _js;
    private readonly AuthService _auth;
    private const string Collection = "medicos";

    public MedicoService(IJSRuntime js, AuthService auth)
    {
        _js = js;
        _auth = auth;
    }

    public async Task<List<Medico>> GetAllAsync()
    {
        var result = await _js.InvokeAsync<JsonElement>("firestore.query", Collection, new
        {
            filters = new[] { new { field = "userId", op = "==", value = _auth.CurrentUser?.Uid } }
        });

        if (!result.GetProperty("success").GetBoolean()) return new();

        var list = new List<Medico>();
        foreach (var item in result.GetProperty("data").EnumerateArray())
        {
            list.Add(MapFromJson(item));
        }
        return list.OrderBy(m => m.Nombre).ToList();
    }

    public async Task<Medico?> GetByIdAsync(string id)
    {
        var result = await _js.InvokeAsync<JsonElement>("firestore.getById", Collection, id);
        if (!result.GetProperty("success").GetBoolean()) return null;
        return MapFromJson(result.GetProperty("data"));
    }

    public async Task<(bool Success, string? Error, string? Id)> AddAsync(Medico entity)
    {
        entity.UserId = _auth.CurrentUser?.Uid ?? string.Empty;
        entity.CreatedAt = DateTime.UtcNow.ToString("o");

        var result = await _js.InvokeAsync<JsonElement>("firestore.add", Collection, MapToDict(entity));
        if (result.GetProperty("success").GetBoolean())
            return (true, null, result.GetProperty("id").GetString());
        return (false, result.GetProperty("error").GetString(), null);
    }

    public async Task<(bool Success, string? Error)> UpdateAsync(Medico entity)
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

    private static Medico MapFromJson(JsonElement el)
    {
        return new Medico
        {
            Id = el.TryGetProperty("id", out var id) ? id.GetString() ?? "" : "",
            Nombre = el.TryGetProperty("nombre", out var n) ? n.GetString() ?? "" : "",
            Especialidad = el.TryGetProperty("especialidad", out var e) ? e.GetString() ?? "" : "",
            CedulaProfesional = el.TryGetProperty("cedulaProfesional", out var c) ? c.GetString() ?? "" : "",
            Telefono = el.TryGetProperty("telefono", out var t) ? t.GetString() ?? "" : "",
            Email = el.TryGetProperty("email", out var em) ? em.GetString() ?? "" : "",
            Notas = el.TryGetProperty("notas", out var no) ? no.GetString() ?? "" : "",
            UserId = el.TryGetProperty("userId", out var u) ? u.GetString() ?? "" : "",
            CreatedAt = el.TryGetProperty("createdAt", out var cr) ? cr.GetString() ?? "" : ""
        };
    }

    private static Dictionary<string, object> MapToDict(Medico m)
    {
        return new Dictionary<string, object>
        {
            ["nombre"] = m.Nombre,
            ["especialidad"] = m.Especialidad,
            ["cedulaProfesional"] = m.CedulaProfesional,
            ["telefono"] = m.Telefono,
            ["email"] = m.Email,
            ["notas"] = m.Notas,
            ["userId"] = m.UserId,
            ["createdAt"] = m.CreatedAt
        };
    }
}
