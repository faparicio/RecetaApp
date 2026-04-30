using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using RecetaApp.Models;
using System.Text.Json;

namespace RecetaApp.Services;

public class RecetaService
{
    private readonly IJSRuntime _js;
    private readonly AuthService _auth;
    private const string Collection = "recetas";

    public RecetaService(IJSRuntime js, AuthService auth)
    {
        _js = js;
        _auth = auth;
    }

    public async Task<List<Receta>> GetAllAsync()
    {
        var result = await _js.InvokeAsync<JsonElement>("firestore.query", Collection, new
        {
            filters = new[] { new { field = "userId", op = "==", value = _auth.CurrentUser?.Uid } },
            orderBy = new[] { new { field = "fecha", direction = "desc" } }
        });

        if (!result.GetProperty("success").GetBoolean()) return new();

        var list = new List<Receta>();
        foreach (var item in result.GetProperty("data").EnumerateArray())
        {
            list.Add(MapFromJson(item));
        }
        return list;
    }

    public async Task<List<Receta>> GetByPacienteAsync(string pacienteId)
    {
        var result = await _js.InvokeAsync<JsonElement>("firestore.query", Collection, new
        {
            filters = new object[]
            {
                new { field = "userId", op = "==", value = _auth.CurrentUser?.Uid },
                new { field = "pacienteId", op = "==", value = pacienteId }
            },
            orderBy = new[] { new { field = "fecha", direction = "desc" } }
        });

        if (!result.GetProperty("success").GetBoolean()) return new();

        var list = new List<Receta>();
        foreach (var item in result.GetProperty("data").EnumerateArray())
        {
            list.Add(MapFromJson(item));
        }
        return list;
    }

    public async Task<Receta?> GetByIdAsync(string id)
    {
        var result = await _js.InvokeAsync<JsonElement>("firestore.getById", Collection, id);
        if (!result.GetProperty("success").GetBoolean()) return null;
        return MapFromJson(result.GetProperty("data"));
    }

    public async Task<(bool Success, string? Error, string? Id)> AddAsync(Receta entity)
    {
        entity.UserId = _auth.CurrentUser?.Uid ?? string.Empty;
        entity.CreatedAt = DateTime.UtcNow.ToString("o");

        var result = await _js.InvokeAsync<JsonElement>("firestore.add", Collection, MapToDict(entity));
        if (result.GetProperty("success").GetBoolean())
            return (true, null, result.GetProperty("id").GetString());
        return (false, result.GetProperty("error").GetString(), null);
    }

    public async Task<(bool Success, string? Error)> UpdateAsync(Receta entity)
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

    public async Task<(bool Success, string? Url, string? Error)> UploadFotoAsync(string recetaId, ElementReference fileInput)
    {
        var path = $"recetas/{_auth.CurrentUser?.Uid}/{recetaId}/{DateTime.UtcNow.Ticks}";
        var result = await _js.InvokeAsync<JsonElement>("firebaseStorage.uploadFile", path, fileInput);

        if (result.GetProperty("success").GetBoolean())
            return (true, result.GetProperty("url").GetString(), null);
        return (false, null, result.GetProperty("error").GetString());
    }

    public async Task<(bool Success, string? Error)> DeleteFotoAsync(string path)
    {
        if (string.IsNullOrEmpty(path)) return (true, null);
        var result = await _js.InvokeAsync<JsonElement>("firebaseStorage.deleteFile", path);
        if (result.GetProperty("success").GetBoolean())
            return (true, null);
        return (false, result.GetProperty("error").GetString());
    }

    private static Receta MapFromJson(JsonElement el)
    {
        return new Receta
        {
            Id = el.TryGetProperty("id", out var id) ? id.GetString() ?? "" : "",
            PacienteId = el.TryGetProperty("pacienteId", out var pi) ? pi.GetString() ?? "" : "",
            PacienteNombre = el.TryGetProperty("pacienteNombre", out var pn) ? pn.GetString() ?? "" : "",
            MedicoId = el.TryGetProperty("medicoId", out var mi) ? mi.GetString() ?? "" : "",
            MedicoNombre = el.TryGetProperty("medicoNombre", out var mn) ? mn.GetString() ?? "" : "",
            Fecha = el.TryGetProperty("fecha", out var f) ? f.GetString() ?? "" : "",
            Diagnostico = el.TryGetProperty("diagnostico", out var d) ? d.GetString() ?? "" : "",
            Notas = el.TryGetProperty("notas", out var n) ? n.GetString() ?? "" : "",
            FotoUrl = el.TryGetProperty("fotoUrl", out var fu) ? fu.GetString() ?? "" : "",
            FotoPath = el.TryGetProperty("fotoPath", out var fp) ? fp.GetString() ?? "" : "",
            UserId = el.TryGetProperty("userId", out var u) ? u.GetString() ?? "" : "",
            CreatedAt = el.TryGetProperty("createdAt", out var c) ? c.GetString() ?? "" : ""
        };
    }

    private static Dictionary<string, object> MapToDict(Receta r)
    {
        return new Dictionary<string, object>
        {
            ["pacienteId"] = r.PacienteId,
            ["pacienteNombre"] = r.PacienteNombre,
            ["medicoId"] = r.MedicoId,
            ["medicoNombre"] = r.MedicoNombre,
            ["fecha"] = r.Fecha,
            ["diagnostico"] = r.Diagnostico,
            ["notas"] = r.Notas,
            ["fotoUrl"] = r.FotoUrl,
            ["fotoPath"] = r.FotoPath,
            ["userId"] = r.UserId,
            ["createdAt"] = r.CreatedAt
        };
    }
}
