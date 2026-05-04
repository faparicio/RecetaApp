# Home - Pantalla de Inicio

**Archivo:** `Pages/Home.razor`  
**Ruta:** `/`  
**Versión:** 26.04.30.01

## Descripcion

Pantalla principal de la aplicacion con navegacion a las 4 secciones principales mediante tarjetas interactivas.

## Componentes

- Logo central: cuadrado redondeado con gradiente azul-teal (#1565C0 a #00897B), icono SVG blanco, 80x80px
- 4 tarjetas de navegacion en grid responsive (col-6 mobile, col-lg-3 desktop):
  - **Recetas** (gradiente azul) - `/recetas`
  - **Pacientes** (gradiente verde) - `/pacientes`
  - **Medicos** (gradiente celeste) - `/medicos`
  - **Medicamentos** (gradiente naranja) - `/catalogo`

## Estilos

- Tarjetas con clase `home-card`: bordes redondeados 20px, sombra suave, hover con translateY(-6px)
- Iconos con clase `home-card-icon`: 80x80px (64x64 en mobile), bordes redondeados 22px, sombras de color
- Animacion hover: elevacion de tarjeta + scale(1.08) del icono
- Responsive: en pantallas <575px se reducen padding e iconos

## Dependencias

- `NavigationManager` (inyectado)
- Sin servicios de datos
