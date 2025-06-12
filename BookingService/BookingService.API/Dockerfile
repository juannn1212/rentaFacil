# 1) Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos sólo los csproj de BookingService
COPY BookingService/BookingService.API/BookingService.API.csproj BookingService.API/
COPY BookingService/BookingService.Infrastructure/BookingService.Infrastructure.csproj BookingService.Infrastructure/
COPY BookingService/BookingService.Domain/BookingService.Domain.csproj BookingService.Domain/

# Restauramos sólo el proyecto API
WORKDIR /src/BookingService.API
RUN dotnet restore

# Copiamos el código completo de cada capa
WORKDIR /src
COPY BookingService/BookingService.API/.           BookingService.API/
COPY BookingService/BookingService.Infrastructure/. BookingService.Infrastructure/
COPY BookingService/BookingService.Domain/.         BookingService.Domain/

# Publicamos
WORKDIR /src/BookingService.API
RUN dotnet publish -c Release -o /app/publish

# 2) Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BookingService.API.dll"]
