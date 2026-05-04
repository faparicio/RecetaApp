# Login - Autenticacion

**Archivo:** `Pages/Login.razor`  
**Ruta:** `/login`  
**Versión:** 26.05.04.01

## Descripcion

Pantalla de inicio de sesion y registro con autenticacion Firebase. Layout independiente (`EmptyLayout`) sin menu lateral.

## Funcionalidades

- **Inicio de sesion:** Email y password con validacion
- **Registro:** Toggle a modo registro con campo de confirmar password
- **Google Sign-In:** Boton de inicio con Google via `signInWithPopup`
- **Verificacion de sesion:** Al cargar, verifica si ya hay sesion activa y redirige a Home

## Flujo

1. Al cargar: verifica sesion existente con `AuthService.InitializeAsync()`. Si hay usuario, redirige a `/`
2. Login: valida campos, llama a `firebaseAuth.signIn()`, en exito redirige a `/`
3. Registro: valida campos + confirmacion de password, llama a `firebaseAuth.signUp()`
4. Google: llama a `firebaseAuth.signInWithGoogle()`, redirige en exito

## Traduccion de errores

Metodo `TranslateError()` traduce mensajes de error de Firebase al espanol:
- `auth/user-not-found` → "No existe una cuenta con ese correo"
- `auth/wrong-password` → "Contrasena incorrecta"
- `auth/email-already-in-use` → "Ya existe una cuenta con ese correo"
- `auth/weak-password` → "La contrasena debe tener al menos 6 caracteres"

## Estilos

- Definidos en `Login.razor.css` (scoped CSS)
- Fondo: gradiente de pantalla completa (#1565C0 a #00897B)
- Card: 400px max-width, centrada vertical y horizontalmente
- Boton Google: borde gris, icono SVG inline

## Modelo interno

`LoginModel`: Email (Required, EmailAddress), Password (Required, MinLength 6), ConfirmPassword

## Dependencias

- `AuthService` - Autenticacion Firebase
- `NavigationManager` - Redireccion post-login
