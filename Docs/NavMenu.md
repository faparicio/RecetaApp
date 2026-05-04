# NavMenu - Menu de Navegacion

**Archivo:** `Layout/NavMenu.razor`  
**Versión:** 26.05.04.01

## Descripcion

Menu lateral de navegacion con branding y links a todas las secciones.

## Estructura

- **Header:** Logo (icono caduceo), nombre "RecetaApp" y version dinamica (leida del assembly)
- **Links:**
  - Inicio (`/`) - icono casa
  - Recetas (`/recetas`) - icono documento
  - Pacientes (`/pacientes`) - icono personas
  - Medicos (`/medicos`) - icono credencial
  - Medicamentos (`/catalogo`) - icono capsula
  - Presion Arterial (`/presion-arterial`) - icono pulso cardiaco

## Version dinamica

La version se obtiene de `System.Reflection.Assembly.GetExecutingAssembly().GetName().Version` y se muestra como texto pequeno junto al nombre de la app.

## Estilos

- Definidos en `NavMenu.razor.css` (scoped CSS)
- Iconos personalizados con clases `bi-*-nav-menu`
- Navbar colapsable en mobile
