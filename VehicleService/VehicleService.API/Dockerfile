# 1) Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos sólo los csproj de VehicleService
COPY VehicleService/VehicleService.API/VehicleService.API.csproj VehicleService.API/
COPY VehicleService/VehicleService.Infrastructure/VehicleService.Infrastructure.csproj VehicleService.Infrastructure/
COPY VehicleService/VehicleService.Domain/VehicleService.Domain.csproj VehicleService.Domain/

# Restauramos sólo el proyecto API
WORKDIR /src/VehicleService.API
RUN dotnet restore

# Copiamos el código completo de cada capa
WORKDIR /src
COPY VehicleService/VehicleService.API/.           VehicleService.API/
COPY VehicleService/VehicleService.Infrastructure/. VehicleService.Infrastructure/
COPY VehicleService/VehicleService.Domain/.         VehicleService.Domain/

# Publicamos
WORKDIR /src/VehicleService.API
RUN dotnet publish -c Release -o /app/publish

# 2) Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "VehicleService.API.dll"]
