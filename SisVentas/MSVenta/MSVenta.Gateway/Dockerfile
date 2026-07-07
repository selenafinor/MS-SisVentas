# ─── Etapa 1: Compilar ──────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src

# Copiamos solo el .csproj primero (aprovecha el cache de Docker:
# si no cambian las dependencias, no se vuelve a descargar todo)
COPY MSVenta.Gateway.csproj ./
RUN dotnet restore

# Copiamos el resto del código (incluye ocelot.json) y compilamos
COPY . .
RUN dotnet publish -c Release -o /app/publish

# ─── Etapa 2: Ejecutar ──────────────────────────────
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80
ENTRYPOINT ["dotnet", "MSVenta.Gateway.dll"]