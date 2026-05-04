# Catalogo de Medicamentos

**Archivo:** `Pages/Catalogo/Index.razor`  
**Ruta:** `/catalogo`  
**Versión:** 26.04.30.01

## Descripcion

Catalogo de medicamentos frecuentes. Permite registrar medicamentos con su informacion basica para reutilizarlos al crear recetas.

## Funcionalidades

- **Listado:** Tarjetas `modern-card accent-warning` con nombre comercial, sustancia activa, presentacion e instrucciones
- **Busqueda:** Campo de texto que filtra por nombre o sustancia activa en tiempo real (`oninput`)
- **Crear/Editar:** Formulario inline con campos:
  - Nombre comercial (obligatorio)
  - Sustancia activa
  - Presentacion (ej: Tabletas 500mg, Jarabe 120ml)
  - Instrucciones por defecto
- **Eliminar:** Boton directo sin confirmacion

## Integracion con Recetas

Los medicamentos del catalogo aparecen como dropdown autocompletado al agregar medicamentos en una receta. Al seleccionar del catalogo se llenan automaticamente el nombre y las instrucciones. La dosis, frecuencia y duracion se capturan al momento de agregar a la receta.

## Modelo de datos

`CatalogoMedicamento`: Id, Nombre, SustanciaActiva, Presentacion, Instrucciones, UserId, CreatedAt

## Dependencias

- `CatalogoMedicamentoService` - CRUD Firestore coleccion "catalogoMedicamentos"
