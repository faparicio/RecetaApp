# Presion Arterial - Control de Mediciones

**Archivo:** `Pages/PresionArterial/Index.razor`  
**Ruta:** `/presion-arterial`  
**Versión:** 26.05.04.01

## Descripcion

Pantalla de control de presion arterial con dos vistas: resumen agrupado por paciente y detalle/historial de un paciente especifico.

## Vistas

### Vista resumen (`/presion-arterial`)

Tarjetas agrupadas por paciente mostrando:
- Nombre del paciente
- Ultima lectura (sistolica/diastolica) con clasificacion color-coded
- Pulso de la ultima medicion
- Total de mediciones registradas
- Click en tarjeta navega al historial del paciente

Si no hay mediciones, muestra enlace a pantalla de Pacientes.

### Vista detalle (`/presion-arterial?pacienteId={id}`)

Tabla con el historial completo de mediciones del paciente:
- Columnas: Fecha, Hora, Sistolica, Diastolica, Pulso, Clasificacion, Notas, Acciones
- Boton "+ Nueva Medicion" pre-asignada al paciente (sin selector de paciente en el formulario)
- Boton "← Todos" para volver al resumen
- Nombre del paciente en el encabezado

## Clasificacion automatica

Basada en guias AHA (American Heart Association):

| Sistolica | Diastolica | Clasificacion | Color |
|-----------|------------|---------------|-------|
| < 120 | < 80 | Normal | Verde (success) |
| 120-129 | < 80 | Elevada | Amarillo (warning) |
| 130-139 | 80-89 | Hipertension Etapa 1 | Amarillo (warning) |
| >= 140 | >= 90 | Hipertension Etapa 2 | Rojo (danger) |

La clasificacion se calcula como propiedad del modelo `PresionArterial.Clasificacion` y se muestra como badge color-coded tanto en la vista resumen como en el formulario de captura en tiempo real.

## Formulario de captura

Campos:
- Paciente (select, solo visible si no hay pacienteId en query param)
- Fecha (obligatorio, default fecha actual yyyy-MM-dd)
- Hora (default hora actual HH:mm)
- Sistolica mmHg (obligatorio, numerico)
- Diastolica mmHg (obligatorio, numerico)
- Pulso lpm (numerico, opcional)
- Notas (textarea, para contexto: ayuno, despues de ejercicio, etc.)

Al ingresar sistolica y diastolica se muestra la clasificacion en tiempo real con alerta color-coded.

## Modelo de datos

`PresionArterial`: Id, PacienteId, PacienteNombre, Sistolica (int), Diastolica (int), Pulso (int), Fecha, Hora, Notas, UserId, CreatedAt

Propiedades calculadas:
- `Clasificacion` - string con la clasificacion segun valores
- `ClasificacionColor` - string con clase CSS de Bootstrap (success, warning, danger)

## Coleccion Firestore

`presionArterial` - Mediciones de presion arterial. Filtradas por userId. Ordenadas por fecha y hora descendente.

## Acceso

- Desde el menu lateral: "Presion Arterial" (vista resumen)
- Desde tarjeta de Paciente: boton "Presion" (vista detalle del paciente)

## Dependencias

- `PresionArterialService` - CRUD Firestore coleccion "presionArterial"
- `PacienteService` - Para obtener nombre de pacientes y llenar select
- `NavigationManager` - Para manejar query params y navegacion
