# Sistema de Notas - Audisoft

Aplicación full-stack para la gestión de **Estudiantes**, **Profesores**, **Materias** y **Notas**, desarrollada como prueba técnica.

- **Backend**: API REST en ASP.NET Core 8 (MVC / Web API) en 3 capas, con Entity Framework Core sobre SQL Server.
- **Frontend**: Aplicación Angular 22 (standalone components) con Bootstrap 5.
- **Pruebas**: xUnit para la capa de negocio (Estudiante y Nota).

## Estructura del repositorio

```
.
├── backend/
│   ├── NotasAudisoft.sln
│   ├── NotasAudisoft.Entidades/   # Capa de entidades (modelos de dominio)
│   ├── NotasAudisoft.Datos/       # Capa de datos (DbContext, repositorios, migraciones EF Core)
│   ├── NotasAudisoft.Negocio/     # Capa de negocio (servicios, DTOs, validaciones)
│   ├── NotasAudisoft.Api/         # API REST (controladores, Swagger, Program.cs)
│   └── NotasAudisoft.Tests/       # Pruebas xUnit (Estudiante y Nota)
└── frontend/
    └── notas-audisoft-web/        # Aplicación Angular + Bootstrap
```

## Arquitectura del backend

| Capa | Responsabilidad |
|---|---|
| **Entidades** | Modelos de dominio puros (`Estudiante`, `Profesor`, `Materia`, `Nota`) y tipos comunes (`ResultadoPaginado<T>`). Sin dependencias externas. |
| **Datos** | `NotasDbContext` (EF Core), configuración Fluent API (llaves foráneas, restricción `CHECK` de la nota, datos semilla), repositorio genérico y repositorios específicos por entidad. |
| **Negocio** | Servicios de aplicación (`EstudianteServicio`, `ProfesorServicio`, `MateriaServicio`, `NotaServicio`), DTOs de entrada/salida, validaciones (nombres, rango de nota) y excepciones de negocio. |
| **Api** | Controladores REST, middleware de manejo de errores, configuración de Swagger, CORS e inyección de dependencias. |

La API se expone **únicamente por HTTP** (sin HTTPS), según lo solicitado.

### Modelo de datos

- **Estudiante**: `Id`, `NombreEstudiante`, `NumeroIdentificacion` (`bigint`, único)
- **Profesor**: `Id`, `NombreProfesor`, `NumeroIdentificacion` (`bigint`, único)
- **Materia**: `Id`, `NombreMateria`
- **Nota**: `Id`, `IdMateria` (FK → Materia), `IdProfesor` (FK → Profesor), `IdEstudiante` (FK → Estudiante), `ValorNota` (`decimal(3,1)`, con restricción `CHECK` entre 0 y 5)

`NumeroIdentificacion` tiene una restricción `UNIQUE` a nivel de base de datos en ambas tablas. Además, la capa de negocio valida explícitamente la unicidad **antes** de insertar o actualizar (`EstudianteServicio`/`ProfesorServicio`), devolviendo un mensaje de error claro ("Ya existe un estudiante/profesor registrado con el número de identificación X.") en vez de dejar que falle la restricción de base de datos.

### Resumen de notas (promedio por materia)

El formulario de **Notas** en la columna "Nota" se muestra **el promedio de las notas de cada materia** para un estudiante, junto con botones **"Mostrar Notas"** (para ver el detalle de notas individuales que componen ese promedio) y **"Eliminar"** (para elegir, de ese detalle, cuál nota puntual eliminar).

Como una tabla `Nota` puede tener varias filas para la misma combinación Estudiante + Materia (distintos profesores o distintas evaluaciones), la vista principal de Notas (`GET /api/notas/resumen`) **agrupa las notas por `Estudiante + Materia`** y calcula el promedio (`PromedioNota`) y la cantidad de notas (`CantidadNotas`). La columna `Id` de esa tabla resumen es un número de fila consecutivo (no corresponde al `Id` de una fila física de `Nota`, ya que una fila del resumen puede representar varias notas). El `Id` real de cada nota individual sí se muestra en los modales "Mostrar Notas" y "Eliminar".

## Puesta en marcha — Backend

### Requisitos

- .NET SDK 8.0
- Acceso a una instancia de SQL Server

### Configuración

La cadena de conexión está en `backend/NotasAudisoft.Api/appsettings.json`:

```json
"ConnectionStrings": {
  "NotasDbConnection": "Server=Neptuno;Database=Notas;User Id=BDUsuario;Password=12345;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```
### Crear la Base de datos 
1. Ingrese al Managment de sql server
2. Cree la bd con el nombre de Notas

## Nota sólo la bse de datos sin tablas

Ajústela según su entorno.

### Migraciones (con datos semilla)

```bash
cd backend
dotnet ef database update --project NotasAudisoft.Datos --startup-project NotasAudisoft.Api
```

Esto aplica dos migraciones: la inicial (tablas `Estudiante`, `Profesor`, `Materia` y `Nota`, con sus llaves foráneas, la restricción de rango 0–5 en `ValorNota`, y datos de ejemplo — 4 estudiantes, 3 profesores, 4 materias y 10 notas) y `AgregarNumeroIdentificacion` (agrega `NumeroIdentificacion` a `Estudiante` y `Profesor` con un índice único, con valores de semilla para los registros existentes).

### Ejecutar la API

```bash
cd backend/NotasAudisoft.Api
dotnet run
```

La API queda disponible en `http://localhost:5080`. Swagger UI: `http://localhost:5080/swagger`.

### Ejecutar las pruebas

```bash
cd backend
dotnet test NotasAudisoft.Tests/NotasAudisoft.Tests.csproj
```

Las pruebas usan el proveedor InMemory de EF Core y cubren, entre otros casos:

- Validación de nombres (letras, tildes y espacios permitidos; números y caracteres especiales rechazados).
- Validación del rango de la nota (0 a 5, incluyendo límites y valores fuera de rango).
- Reglas de negocio: no se puede eliminar un estudiante/profesor/materia con notas asociadas, el `Id` de ruta debe coincidir con el del cuerpo al actualizar, cálculo del promedio por Estudiante + Materia, etc.

## Endpoints principales

| Recurso | Endpoints |
|---|---|
| Estudiantes | `GET/POST /api/estudiantes`, `GET/PUT/DELETE /api/estudiantes/{id}` |
| Profesores | `GET/POST /api/profesores`, `GET/PUT/DELETE /api/profesores/{id}` |
| Materias | `GET/POST /api/materias`, `GET/PUT/DELETE /api/materias/{id}` |
| Notas | `GET/POST /api/notas`, `GET/PUT/DELETE /api/notas/{id}`, `GET /api/notas/resumen` (promedio paginado), `GET /api/notas/estudiante/{idEstudiante}/materia/{idMateria}` (detalle de notas de una materia) |

Todos los listados son paginados (`pagina`, `tamanoPagina`) y los de Estudiante/Profesor/Materia aceptan `busqueda` para filtrar por nombre.

## Puesta en marcha - Frontend

### Requisitos

- Node.js 20+
- Angular CLI

### Instalación y ejecución

```bash
cd frontend/notas-audisoft-web
npm install
npm start   # equivalente a: ng serve --port 4200
```

La aplicación queda disponible en `http://localhost:4200` y consume la API en `http://localhost:5080/api` (configurado en `src/environments/environment.ts`).

### Pruebas

```bash
npm test
```

### Funcionalidades

- Menú de navegación con **Estudiantes**, **Profesores**, **Materias** y **Notas** (esta última es adicional al alcance mínimo, para poder administrar las materias usadas por las notas).
- Cada formulario incluye: listado paginado, buscador por nombre, creación, edición y eliminación (con modal de confirmación).
- Validación de nombres: solo letras (incluye tildes y "ñ") y espacios; se rechazan números y caracteres especiales, tanto en el formulario (Angular `Validators` + validador personalizado) como en el backend (defensa en profundidad).
- Validación de la nota: solo se aceptan valores entre 0 y 5, tanto en el cliente como en el servidor (incluida la restricción `CHECK` en la base de datos).
- El formulario de Notas muestra el promedio por Estudiante + Materia, con los botones **"Mostrar Notas"** (detalle de notas individuales) y **"Eliminar"** (selección de la nota puntual a eliminar).
- Toda operación de creación, actualización o eliminación muestra una alerta (éxito o error) en la esquina superior derecha.

## Decisiones y supuestos adicionales

- Se usó **.NET 8 (LTS)** en lugar de versiones preliminares disponibles en el entorno, por estabilidad.
- Se agregó la entidad **Materia** como una cuarta sección del menú (no solicitada explícitamente en el punto 3, pero necesaria para poder crear/editar materias que luego se usan en el formulario de Notas).
- Las contraseñas y cadenas de conexión de este repositorio corresponden a un entorno de prueba y no deben reutilizarse en producción.
