# Recetas Medicas

**Archivo:** `Pages/Recetas/Index.razor`  
**Ruta:** `/recetas`  
**Versión:** 26.05.22.01

## Descripcion

Pantalla principal y mas compleja de la app. Permite crear, ver, editar, eliminar y exportar recetas medicas con sus medicamentos asociados.

## Funcionalidades

### Listado
- Tarjetas `modern-card accent-primary clickable-card` con nombre del paciente, fecha, medico, edad y diagnostico
- Filtrado por paciente via query param `?pacienteId={id}` (navegacion desde pantalla Pacientes)
- Boton para quitar filtro

### Numeracion de medicamentos
Las tablas de medicamentos (formulario de creacion/edicion, vista detalle y exportacion PDF) incluyen una columna `#` con numeracion automatica secuencial.

### Crear/Editar Receta
Formulario inline con campos:
- Paciente (select, obligatorio)
- Medico (select)
- Fecha (obligatorio, default hoy en formato yyyy-MM-dd)
- Peso (kg), Talla (cm)
- Diagnostico
- Notas (textarea)
- **Medicamentos inline:** Se agregan medicamentos desde el formulario antes de guardar. Cada medicamento tiene:
  - Nombre (con dropdown autocompletado del catalogo)
  - Dosis (ej: 500mg)
  - Frecuencia (ej: Cada 8 horas)
  - Duracion (ej: 7 dias)
  - Instrucciones (ej: Tomar con alimentos)
- Al guardar se calcula automaticamente la edad del paciente segun la fecha de la receta

### Vista Detalle
- Se accede haciendo clic en la tarjeta de la receta
- Muestra toda la informacion de la receta con tabla de medicamentos
- Permite agregar medicamentos adicionales con dropdown del catalogo
- Boton para exportar a PDF
- Notas con `white-space: pre-line` para respetar saltos de linea

### Exportar a PDF
- Genera HTML string con `StringBuilder` en C#
- Pasa el HTML a `window.exportToPdf()` via JS interop
- Usa html2pdf.js con modo `.from(htmlContent, 'string')`
- Fallback: si falla, abre HTML en nueva pestana como blob
- **Nota:** El PDF ha presentado problemas de paginas en blanco. Pendiente de validar.

### Edicion de receta
- Carga los medicamentos existentes en la tabla inline
- Al guardar: elimina todos los medicamentos existentes y re-agrega los de la lista (delete + re-add)

## Calculo automatico de edad

Metodo `CalcularEdadEnFecha()` calcula la edad del paciente a la fecha de la receta (no a la fecha actual). Se guarda en el campo `EdadPaciente` de la receta.

## Consultas Firestore

Las consultas no usan `orderBy` en Firestore para evitar el requisito de indices compuestos. El ordenamiento se hace en C# con `.OrderByDescending(r => r.Fecha)`.

## Modelos de datos

- `Receta`: Id, PacienteId, PacienteNombre, MedicoId, MedicoNombre, Fecha, Peso, Talla, Diagnostico, Notas, EdadPaciente, UserId, CreatedAt
- `Medicamento`: Id, RecetaId, Nombre, Dosis, Frecuencia, Duracion, Instrucciones

## Dependencias

- `RecetaService` - CRUD Firestore coleccion "recetas"
- `PacienteService` - Para llenar select de pacientes
- `MedicoService` - Para llenar select de medicos
- `MedicamentoService` - CRUD Firestore coleccion "medicamentos"
- `CatalogoMedicamentoService` - Para dropdown autocompletado
- `NavigationManager` - Para manejar query params
- `IJSRuntime` - Para exportar a PDF via JS interop
