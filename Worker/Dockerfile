# 1) Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar todo el repositorio para que estén presentes BookingService.Infrastructure y BookingService.Domain
COPY . .

# Restaurar y publicar sólo el proyecto Worker
WORKDIR /src/Worker
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish

# 2) Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Worker.dll"]
