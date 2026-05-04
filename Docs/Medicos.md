# Medicos

**Archivo:** `Pages/Medicos/Index.razor`  
**Ruta:** `/medicos`  
**Versión:** 26.04.30.01

## Descripcion

CRUD de medicos. Directorio de medicos con informacion de contacto y ubicacion de consultorio.

## Funcionalidades

- **Listado:** Tarjetas `modern-card accent-info` con nombre, especialidad (badge), cedula, telefono, ubicacion (enlace a Google Maps) y notas
- **Crear/Editar:** Formulario inline con campos:
  - Nombre completo (obligatorio)
  - Especialidad
  - Cedula profesional
  - Telefono
  - Email
  - Ubicacion del consultorio (texto libre, se usa para generar enlace a Google Maps)
  - Notas (textarea)
- **Eliminar:** Boton directo sin confirmacion

## Ubicacion con mapa

La ubicacion se muestra como badge verde con icono de pin. Al hacer clic abre Google Maps con la busqueda del texto ingresado usando `Uri.EscapeDataString()`.

## Modelo de datos

`Medico`: Id, Nombre, Especialidad, CedulaProfesional, Telefono, Email, Ubicacion, Notas, UserId, CreatedAt

## Dependencias

- `MedicoService` - CRUD Firestore coleccion "medicos"
