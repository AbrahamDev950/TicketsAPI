# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copiar archivos del proyecto
COPY ["AtiendiTicketsAPI.csproj", "./"]

# Restaurar dependencias
RUN dotnet restore "AtiendiTicketsAPI.csproj"

# Copiar el código fuente
COPY . .

# Compilar la aplicación
RUN dotnet build "AtiendiTicketsAPI.csproj" -c Release -o /app/build

# Etapa 2: Publish
FROM build AS publish

RUN dotnet publish "AtiendiTicketsAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

# Copiar la aplicación compilada
COPY --from=publish /app/publish .

# Exponer puerto
EXPOSE 8080

# Variables de entorno
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Comando para ejecutar la aplicación
ENTRYPOINT ["dotnet", "AtiendiTicketsAPI.dll"]
