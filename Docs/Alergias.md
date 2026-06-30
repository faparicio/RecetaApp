# Alergias - Control por Paciente

**Archivo:** `Pages/Alergias/Index.razor`
**Ruta:** `/alergias`
**Versión:** 26.06.30.01

## Descripcion

Pantalla de control de alergias estructuradas por paciente, con dos vistas: resumen agrupado por paciente y detalle/listado de un paciente especifico. Reemplaza el registro de alergias como texto libre por registros individuales con severidad y reaccion.

## Vistas

### Vista resumen (`/alergias`)

Tarjetas agrupadas por paciente mostrando:
- Nombre del paciente
- Badges con las primeras 4 alergias (color segun severidad); si hay mas, badge `+N`
- Total de alergias registradas
- El color de acento de la tarjeta refleja la severidad maxima del paciente
- Click en tarjeta navega al detalle del paciente

Si no hay alergias, muestra enlace a la pantalla de Pacientes.

### Vista detalle (`/alergias?pacienteId={id}`)

Tabla con las alergias del paciente:
- Columnas: Alergia, Severidad, Reaccion, Notas, Acciones
- Boton "+ Nueva Alergia" pre-asignada al paciente (sin selector de paciente en el formulario)
- Boton "← Todos" para volver al resumen
- Nombre del paciente en el encabezado

## Severidad

| Severidad | Color |
|-----------|-------|
| Leve | Verde (success) |
| Moderada | Amarillo (warning) |
| Severa | Rojo (danger) |

La severidad se captura con un select (default "Leve") y se muestra como badge color-coded en la tabla, el resumen y una alerta en tiempo real dentro del formulario. Las alergias se ordenan de mayor a menor severidad (`SeveridadPeso`) y luego por nombre.

## Formulario de captura

Campos:
- Paciente (select, solo visible si no hay pacienteId en query param)
- Alergia (obligatorio, ej: Penicilina, Mariscos, Polen)
- Severidad (select: Leve / Moderada / Severa)
- Reaccion (ej: Urticaria, Dificultad para respirar, Hinchazon)
- Notas (textarea)

## Modelo de datos

`Alergia`: Id, PacienteId, PacienteNombre, Nombre, Severidad, Reaccion, Notas, UserId, CreatedAt

Propiedades calculadas:
- `SeveridadColor` - clase CSS de Bootstrap (success, warning, danger, secondary)
- `SeveridadPeso` - entero (Severa=3, Moderada=2, Leve=1) para ordenar

## Coleccion Firestore

`alergias` - Alergias por paciente. Filtradas por userId. Ordenadas por severidad (desc) y nombre.

## Acceso

- Desde el menu lateral: "Alergias" (vista resumen)
- Desde Home: tarjeta "Alergias" (gradiente ambar)
- Desde tarjeta de Paciente: boton "Alergias" (vista detalle del paciente)

## Dependencias

- `AlergiaService` - CRUD Firestore coleccion "alergias"
- `PacienteService` - Para obtener nombre de pacientes y llenar select
- `NavigationManager` - Para manejar query params y navegacion
