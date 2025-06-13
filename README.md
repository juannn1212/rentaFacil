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

Desde la carpeta `client` (o `client`, según tu estructura):

```bash
cd client
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

## 3. Despliegue en Azure (AKS, Azure SQL y CI/CD)

A continuación se describe cómo desplegar los microservicios y la base de datos en Azure, sin implementación práctica, pero con la arquitectura y pasos necesarios:

### 3.1 Azure Container Registry (ACR) y AKS

1. **ACR**: Crear un Azure Container Registry (`MyRentaFacilACR`) y en el pipeline CI construir y pushear las imágenes:

   ```bash
   az acr build --registry MyRentaFacilACR --image vehiclesvc:$(Build.BuildId) VehicleService/VehicleService.API
   az acr build --registry MyRentaFacilACR --image bookingsvc:$(Build.BuildId) BookingService/BookingService.API
   az acr build --registry MyRentaFacilACR --image worker:$(Build.BuildId) Worker
   ```
2. **AKS**: Crear un cluster AKS en un Resource Group:

   ```bash
   az aks create --resource-group RentaFacil-RG --name renta-facil-aks \
     --node-count 3 --enable-addons monitoring --generate-ssh-keys
   az aks get-credentials --resource-group RentaFacil-RG --name renta-facil-aks
   ```
3. **Despliegue**: Usar manifests (o Helm) para cada servicio, referenciando las imágenes en ACR y un Secret de conexión a Azure SQL:

   ```yaml
   # vehiclesvc-deployment.yaml
   apiVersion: apps/v1
   kind: Deployment
   metadata: { name: vehiclesvc }
   spec:
     replicas: 2
     selector: { matchLabels: { app: vehiclesvc } }
     template:
       metadata: { labels: { app: vehiclesvc } }
       spec:
         containers:
         - name: vehiclesvc
           image: myrentaFacilACR.azurecr.io/vehiclesvc:$(Build.BuildId)
           env:
           - name: ConnectionStrings__DefaultConnection
             valueFrom:
               secretKeyRef:
                 name: sql-credentials
                 key: SqlConnection
   ```

   Y exponerlos con un Ingress NGINX para rutas `/api/vehicles` y `/api/bookings`.

### 3.2 Azure SQL

1. **Servidor y bases**:

   ```bash
   az sql server create --name renta-facil-sqlsrv --resource-group RentaFacil-RG \
     --location eastus --admin-user sqladmin --admin-password <StrongP@ssw0rd>
   az sql db create --resource-group RentaFacil-RG --server renta-facil-sqlsrv \
     --name VehicleServiceDb --service-objective S0
   az sql db create --resource-group RentaFacil-RG --server renta-facil-sqlsrv \
     --name BookingServiceDb --service-objective S0
   ```
2. **Red y firewall**: Permitir acceso desde el rango de IP del AKS o usar Private Endpoint.
3. **Secretos**: Almacenar la cadena de conexión en Key Vault o como Secret de Kubernetes:

   ```bash
   kubectl create secret generic sql-credentials --from-literal=SqlConnection="Server=tcp:renta-facil-sqlsrv.database.windows.net;Database=VehicleServiceDb;User ID=sqladmin;Password=..."
   ```

### 3.3 CI/CD con Azure DevOps

1. **CI Pipeline** en Azure DevOps (`azure-pipelines-ci.yml`): restaurar, construir, testear `.NET`, luego `Docker@2` para `buildAndPush` las imágenes a ACR.
2. **CD Pipeline** (`azure-pipelines-cd.yml`): desplegar con `Kubernetes@1` apuntando a los manifests en el repositorio para actualizar las cargas en AKS.
3. **Artefactos y aprobaciones**: CI publica imágenes en ACR, CD despliega en entornos con aprobación manual entre pre-prod y prod.

Con esta configuración obtenemos:

* Imágenes versionadas en ACR.
* Despliegue automatizado en AKS.
* Base de datos gestionada en Azure SQL.
* CI/CD sólido con Azure DevOps.

---
