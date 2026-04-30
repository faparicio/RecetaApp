# CLAUDE.md

This file provides guidance to Claude Code when working with code in this repository.

## Exclusiones
Este proyecto NO utiliza el workflow de Waykee (no crear tickets, no buscar RAG, no documentar en Waykee). Ignorar las reglas de Waykee del CLAUDE.md padre.

## Build & Run Commands

```bash
# Build the project
dotnet build

# Run development server (launches at https://localhost:7029)
dotnet run

# Publish for production
dotnet publish -c Release
```

## Architecture Overview

**Framework:** .NET 9.0 Blazor WebAssembly (browser-wasm) — PWA instalable
**Backend:** Firebase (Firestore, Authentication, Storage) via JavaScript interop
**UI:** Bootstrap 5

### Project Structure

- **Models/** - Data entities (Paciente, Medico, Receta, Medicamento)
- **Services/** - Business logic layer; each service handles Firebase CRUD via IJSRuntime
- **Pages/** - Razor components organized by feature (Pacientes/, Medicos/, Recetas/)
- **Layout/** - MainLayout, EmptyLayout, NavMenu components
- **wwwroot/** - Static assets, Firebase SDK integration in index.html, PWA manifest

### Key Patterns

**Service Pattern:**
- Services inject `IJSRuntime` for JavaScript interop with Firebase
- Standard methods: `GetAllAsync()`, `GetByIdAsync(id)`, `AddAsync(entity)`, `UpdateAsync(entity)`, `DeleteAsync(id)`
- Return tuples for operations: `(bool Success, string? Error)`
- All queries filter by `userId` for data isolation

**Firebase JavaScript APIs (defined in index.html):**
- `window.firebaseAuth.*` - Authentication (email/password)
- `window.firestore.*` - Firestore CRUD with query support
- `window.firebaseStorage.*` - File storage (fotos de recetas)

**Data Model:**
- Properties default to empty strings
- Timestamps use `DateTime.UtcNow.ToString("o")`
- All entities include `UserId` for per-user data isolation

### Firebase Setup
El archivo `index.html` contiene placeholders para la configuración de Firebase.
Para activar la app, crear un proyecto en Firebase Console y reemplazar los valores en `firebaseConfig`.
