# Pacientes

**Archivo:** `Pages/Pacientes/Index.razor`  
**Ruta:** `/pacientes`  
**Versión:** 26.04.30.01

## Descripcion

CRUD de pacientes (familiares). Permite registrar nombre, fecha de nacimiento, alergias y notas.

## Funcionalidades

- **Listado:** Tarjetas `modern-card accent-success` con nombre, edad calculada (anos y meses), alergias y notas
- **Crear/Editar:** Formulario inline con campos:
  - Nombre completo (obligatorio)
  - Fecha de nacimiento (texto libre dd/mm/aaaa)
  - Alergias
  - Notas (textarea)
- **Eliminar:** Boton directo sin confirmacion
- **Navegacion:** Boton "Recetas" en cada tarjeta que navega a `/recetas?pacienteId={id}`

## Calculo de edad

Metodo `CalcularEdadTexto()` calcula edad con anos y meses, con pluralizacion en espanol:
- "1 ano 3 meses", "2 anos", "5 meses"

## Modelo de datos

`Paciente`: Id, Nombre, FechaNacimiento, Alergias, Notas, UserId, CreatedAt

## Dependencias

- `PacienteService` - CRUD Firestore coleccion "pacientes"
- `NavigationManager` - para navegar a recetas filtradas
