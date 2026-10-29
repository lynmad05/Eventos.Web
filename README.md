# Eventos.Web · EF Core Code First

Aplicación web de **eventos e inscripciones** desarrollada con **ASP.NET Core MVC** y **Entity Framework Core (Code First)**. La base de datos `EventosDB` se genera a partir de las clases C# mediante migraciones, sin scripts SQL ni procedimientos almacenados. Todo el acceso a datos se resuelve con LINQ.

## Datos del curso

| | |
|---|---|
| **Curso** | Desarrollo de Aplicaciones Empresariales Avanzadas |
| **Sección** | 6 - C24 - Sección C |
| **Docente** | Edwin William Arévalo Sermeño |
| **Laboratorio** | EF Core Code First: Eventos e inscripciones (Semana 11) |
| **Institución** | TECSUP |

## Tecnologías

- .NET 10 · ASP.NET Core MVC (Razor)
- Entity Framework Core (SQL Server) con Code First y Data Annotations
- SQL Server
- Bootstrap 5 · Lucide Icons
- Patrón Repositorio con inyección de dependencias

## Funcionalidades

- **Eventos:** listado de eventos activos con su categoría, filtro por categoría, orden por fecha, inscritos confirmados y cupos disponibles. Detalle, creación, edición y eliminación lógica.
- **Asistentes:** listado de asistentes activos ordenados por nombres y registro con validación de DNI y correo únicos.
- **Inscripciones:** listado con evento, asistente, fecha y estado. Registro con validaciones y cancelación de inscripciones confirmadas.
- **Datos iniciales:** carga automática de 3 categorías, 4 eventos y 5 asistentes si las tablas están vacías.
- **Mensajes de resultado** con `TempData` y patrón Post/Redirect/Get.
  
### Restricciones principales

| Entidad | Restricciones |
|---|---|
| **Categoria** | `Nombre` obligatorio, máx. 50, único |
| **Evento** | `Titulo` y `Lugar` obligatorios (máx. 100), `Aforo` entre 1 y 5000, `Precio` decimal(10,2) no negativo, `Modalidad` Presencial o Virtual |
| **Asistente** | `Nombres` obligatorio, `Dni` de 8 dígitos único, `Correo` con formato válido y único, `Telefono` opcional (máx. 9) |
| **Inscripcion** | `Estado` Confirmada o Cancelada |

Categoria, Evento y Asistente usan **eliminación lógica** (`Activo = false`). En Inscripcion, cancelar cambia el `Estado` a `Cancelada`.

## Estructura del proyecto

```
Eventos.Web/
├── Controllers/
│   ├── AsistentesController.cs
│   ├── EventosController.cs
│   ├── HomeController.cs
│   └── InscripcionesController.cs
├── Data/
│   └── EventosContext.cs
├── Migrations/
├── Models/
│   ├── Asistente.cs
│   ├── Categoria.cs
│   ├── Evento.cs
│   └── Inscripcion.cs
├── Repositorios/
│   ├── AsistenteRepositorio.cs
│   ├── EventoRepositorio.cs
│   └── InscripcionRepositorio.cs
├── Views/
│   ├── Asistentes/
│   ├── Eventos/
│   ├── Inscripciones/
│   └── Shared/
├── wwwroot/
├── appsettings.json
└── Program.cs
```

## Requisitos

- [.NET SDK 10](https://dotnet.microsoft.com/download)
- SQL Server (local o Express)
- Visual Studio Code
- Herramienta `dotnet-ef`

```bash
dotnet tool install --global dotnet-ef
```
## Migraciones

La carpeta `Migrations` contiene exactamente tres migraciones, tal como se aplicaron.

| # | Migración | Cambio |
|---|---|---|
| 1 | `InitialCreate` | Crea `EventosDB` con Categorias, Eventos, Asistentes e Inscripciones |
| 2 | `AgregarModalidadEvento` | Agrega `Modalidad` a Evento. `defaultValue` ajustado a `"Presencial"` para conservar los eventos ya registrados |
| 3 | `AgregarTelefonoAsistente` | Agrega `Telefono` (opcional, máx. 9) a Asistente |

### Comandos utilizados

```bash
dotnet tool install --global dotnet-ef

dotnet ef migrations add InitialCreate
dotnet ef database update

dotnet ef migrations add AgregarModalidadEvento
dotnet ef database update

dotnet ef migrations add AgregarTelefonoAsistente
dotnet ef database update
```

### Verificación en SQL Server

```sql
SELECT MigrationId, ProductVersion FROM __EFMigrationsHistory ORDER BY MigrationId;
```

## Reglas de negocio de las inscripciones

- No se puede inscribir en un evento que ya pasó.
- Un asistente no puede estar inscrito dos veces (Confirmada) en el mismo evento.
- No se permite inscribir si las inscripciones confirmadas ya alcanzaron el aforo.
- Solo se pueden cancelar las inscripciones en estado `Confirmada`.

## Autora

**Ailyn Medina Mallqui**
