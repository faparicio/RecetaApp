# Arquitectura General - RecetaApp

**Versión:** 26.05.04.01

## Stack Tecnologico

- **Framework:** .NET 9.0 Blazor WebAssembly (browser-wasm)
- **Tipo:** PWA (Progressive Web App) instalable
- **Backend:** Firebase (Firestore, Authentication)
- **Hosting:** Firebase Hosting (https://recetaapp-ab3f0.web.app)
- **UI:** Bootstrap 5 + CSS custom moderno
- **PDF:** html2pdf.js via JS interop

## Patron de Servicios

Cada entidad tiene un servicio que inyecta `IJSRuntime` para interoperar con las APIs de Firebase definidas en `index.html`:
- `AuthService` - Autenticacion (email/password)
- `PacienteService` - CRUD pacientes
- `MedicoService` - CRUD medicos
- `RecetaService` - CRUD recetas
- `MedicamentoService` - CRUD medicamentos
- `CatalogoMedicamentoService` - CRUD catalogo de medicamentos
- `PresionArterialService` - CRUD mediciones de presion arterial

Patron comun: `GetAllAsync()`, `AddAsync()`, `UpdateAsync()`, `DeleteAsync()`  
Retorno de operaciones: `(bool Success, string? Error)` o `(bool, string?, string? Id)`

## Aislamiento de datos

Todas las entidades incluyen campo `UserId`. Las consultas filtran por userId para aislar datos por usuario.

## Colecciones Firestore

- `pacientes` - Pacientes/familiares
- `medicos` - Directorio de medicos
- `recetas` - Recetas medicas
- `medicamentos` - Medicamentos por receta (relacion via RecetaId)
- `catalogoMedicamentos` - Catalogo de medicamentos frecuentes
- `presionArterial` - Mediciones de presion arterial por paciente

## PWA

- Manifest: `manifest.webmanifest` con iconos 512px y 192px
- Service Worker: cache offline de assets estaticos
- Iconos: gradiente azul-teal con simbolo Rx

## Deployment

```bash
dotnet publish -c Release
firebase deploy --only hosting --account fahz.dev.26@gmail.com
```

Cuenta Firebase: fahz.dev.26@gmail.com  
Proyecto: recetaapp-ab3f0

## Archivos de configuracion

- `firebase.json` - Config de hosting, rewrites SPA, cache headers
- `.firebaserc` - Proyecto Firebase por defecto
- `RecetaApp.csproj` - Version, dependencias .NET
- `wwwroot/index.html` - Firebase SDK, funciones JS interop, html2pdf.js
