# RentaFácil - Microservicios y Frontend Angular

Este repositorio contiene:

* **VehicleService**: Microservicio para registrar y consultar vehículos.
* **BookingService**: Microservicio para crear reservas y consultar historial.
* **Worker**: Servicio en background que genera reportes diarios.
* **Frontend Angular**: Interfaz de usuario para consumir los microservicios.

## Requisitos

* Docker y Docker Compose
* .NET 8 SDK
* Node.js (≥22.12)
* Angular CLI (`npm install -g @angular/cli`)

## 1. Levantar los servicios con Docker

En la raíz del proyecto ejecuta:

```bash
# Construir y levantar contenedores en segundo plano
docker-compose up -d --build

# Verificar estado
docker-compose ps

# Consultar logs si alguna pieza falla
docker-compose logs -f sqlserver
docker-compose logs -f vehiclesvc
docker-compose logs -f bookingsvc
docker-compose logs -f worker
```

> **Nota**: El contenedor de frontend **no** se genera en Docker.

## 2. Generar y aplicar migraciones de base de datos

Para versionar y recrear la estructura de tablas, genera migraciones y luego aplícalas:

### VehicleService

```bash
dotnet ef migrations add InitialCreate \
  --project VehicleService/VehicleService.Infrastructure/VehicleService.Infrastructure.csproj \
  --startup-project VehicleService/VehicleService.API/VehicleService.API.csproj \
  --context VehicleDbContext \
  --output-dir Migrations
```

### BookingService

```bash
dotnet ef migrations add InitialCreate \
  --project BookingService/BookingService.Infrastructure/BookingService.Infrastructure.csproj \
  --startup-project BookingService/BookingService.API/BookingService.API.csproj \
  --context BookingDbContext \
  --output-dir Migrations
```

Luego, para aplicar todas las migraciones pendientes:

```bash
# VehicleService
dotnet ef database update \
  --project VehicleService/VehicleService.Infrastructure/VehicleService.Infrastructure.csproj \
  --startup-project VehicleService/VehicleService.API/VehicleService.API.csproj \
  --context VehicleDbContext

# BookingService
dotnet ef database update \
  --project BookingService/BookingService.Infrastructure/BookingService.Infrastructure.csproj \
  --startup-project BookingService/BookingService.API/BookingService.API.csproj \
  --context BookingDbContext
```

> Estas migraciones crean las tablas necesarias y la historia de EF.

## 3. Ejecutar las APIs .NET

Con las migraciones aplicadas, arranca cada API manualmente:

```bash
# VehicleService (puerto 5010)
cd VehicleService/VehicleService.API
dotnet run
# Escucha en http://localhost:5010

# BookingService (puerto 5045)
cd ../../BookingService/BookingService.API
dotnet run
# Escucha en http://localhost:5045
```

## 4. Ejecutar el Worker

```bash
cd ../../Worker
dotnet run
```

El Worker se conecta a BookingService para generar reportes diarios.

## 5. Ejecutar el Frontend Angular

Desde la carpeta `renta-facil-client` (o `client`, según tu estructura):

```bash
cd renta-facil-client
npm install           # instalar dependencias
ng serve              # levantar en http://localhost:4200
```

Navega a `http://localhost:4200` y prueba las rutas:

* `/search`  → Buscar vehículos por tipo y fecha
* `/book`    → Crear reserva (elige vehículo de la lista)
* `/history` → Ver historial por Client ID

## 6. CORS y configuraciones finales

Asegúrate de tener en cada API (`Program.cs`):

```csharp
// Habilitar CORS para Angular Dev
builder.Services.AddCors(opt => {
  opt.AddPolicy("AllowAngularDev", p => p
    .WithOrigins("http://localhost:4200")
    .AllowAnyHeader()
    .AllowAnyMethod()
  );
});

app.UseCors("AllowAngularDev");
```

Con esto el frontend podrá comunicar sin problemas.

---

¡Listo! Con estos pasos tendrás tu ecosistema de microservicios y frontend funcionando correctamente.
