using Microsoft.JSInterop;
using RecetaApp.Models;
using System.Text.Json;

namespace RecetaApp.Services;

public class PacienteService
{
    private readonly IJSRuntime _js;
    private readonly AuthService _auth;
    private const string Collection = "pacientes";

    public PacienteService(IJSRuntime js, AuthService auth)
    {
        _js = js;
        _auth = auth;
    }

    public async Task<List<Paciente>> GetAllAsync()
    {
        var result = await _js.InvokeAsync<JsonElement>("firestore.query", Collection, new
        {
            filters = new[] { new { field = "userId", op = "==", value = _auth.CurrentUser?.Uid } }
        });

        if (!result.GetProperty("success").GetBoolean()) return new();

        var list = new List<Paciente>();
        foreach (var item in result.GetProperty("data").EnumerateArray())
        {
            list.Add(MapFromJson(item));
        }
        return list.OrderBy(p => p.Nombre).ToList();
    }

    public async Task<Paciente?> GetByIdAsync(string id)
    {
        var result = await _js.InvokeAsync<JsonElement>("firestore.getById", Collection, id);
        if (!result.GetProperty("success").GetBoolean()) return null;
        return MapFromJson(result.GetProperty("data"));
    }

    public async Task<(bool Success, string? Error, string? Id)> AddAsync(Paciente entity)
    {
        entity.UserId = _auth.CurrentUser?.Uid ?? string.Empty;
        entity.CreatedAt = DateTime.UtcNow.ToString("o");

        var result = await _js.InvokeAsync<JsonElement>("firestore.add", Collection, MapToDict(entity));
        if (result.GetProperty("success").GetBoolean())
            return (true, null, result.GetProperty("id").GetString());
        return (false, result.GetProperty("error").GetString(), null);
    }

    public async Task<(bool Success, string? Error)> UpdateAsync(Paciente entity)
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

    private static Paciente MapFromJson(JsonElement el)
    {
        return new Paciente
        {
            Id = el.TryGetProperty("id", out var id) ? id.GetString() ?? "" : "",
            Nombre = el.TryGetProperty("nombre", out var n) ? n.GetString() ?? "" : "",
            FechaNacimiento = el.TryGetProperty("fechaNacimiento", out var fn) ? fn.GetString() ?? "" : "",
            Alergias = el.TryGetProperty("alergias", out var a) ? a.GetString() ?? "" : "",
            Notas = el.TryGetProperty("notas", out var no) ? no.GetString() ?? "" : "",
            UserId = el.TryGetProperty("userId", out var u) ? u.GetString() ?? "" : "",
            CreatedAt = el.TryGetProperty("createdAt", out var c) ? c.GetString() ?? "" : ""
        };
    }

    private static Dictionary<string, object> MapToDict(Paciente p)
    {
        return new Dictionary<string, object>
        {
            ["nombre"] = p.Nombre,
            ["fechaNacimiento"] = p.FechaNacimiento,
            ["alergias"] = p.Alergias,
            ["notas"] = p.Notas,
            ["userId"] = p.UserId,
            ["createdAt"] = p.CreatedAt
        };
    }
}
