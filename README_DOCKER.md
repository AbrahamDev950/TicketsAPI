# AtiendiTicketsAPI - Docker Setup

Esta guía explica cómo ejecutar la API de tickets usando Docker.

## 📋 Requisitos previos

- Docker instalado ([Descargar Docker Desktop](https://www.docker.com/products/docker-desktop))
- Docker Compose (incluido en Docker Desktop)

## 🚀 Opciones de ejecución

### Opción 1: Usando Docker Compose (RECOMENDADO - Más fácil)

```bash
# Clona o descarga el repositorio
cd AtiendiTicketsAPI

# Construye la imagen y ejecuta el contenedor
docker-compose up --build

# La API estará disponible en: http://localhost:8080
```

**Detener la API:**
```bash
docker-compose down
```

---

### Opción 2: Usando Docker directamente

```bash
# Construir la imagen
docker build -t atendi-tickets-api .

# Ejecutar el contenedor
docker run -d -p 8080:8080 \
  -v atendi-data:/app/data \
  --name atendi-api \
  atendi-tickets-api

# La API estará disponible en: http://localhost:8080
```

**Detener el contenedor:**
```bash
docker stop atendi-api
docker rm atendi-api
```

---

## 📍 Acceder a la API

Una vez que el contenedor esté corriendo:

- **Swagger UI:** http://localhost:8080/swagger/ui/index.html
- **API Base:** http://localhost:8080/api/tickets

## 📝 Ejemplo de uso

### Crear un ticket (POST)
```bash
curl -X POST http://localhost:8080/api/tickets \
  -H "Content-Type: application/json" \
  -d '{
    "cliente": "Juan Pérez",
    "asunto": "Login no funciona",
    "descripcion": "No puedo acceder a mi cuenta",
    "prioridad": "Alta",
    "estado": "Abierto"
  }'
```

### Obtener todos los tickets (GET)
```bash
curl http://localhost:8080/api/tickets
```

### Obtener un ticket específico (GET)
```bash
curl http://localhost:8080/api/tickets/{id}
```

## 🔄 Persistencia de datos

La base de datos SQLite se guarda en:
- **En Docker Compose:** `./data/AtiendiTicketsDB.db` (carpeta local)
- **En Docker directo:** Volumen `atendi-data`

Los datos **persisten** incluso si paras y reiniciar el contenedor.

## 🐛 Solución de problemas

### "Puerto 8080 ya está en uso"
Usa otro puerto:
```bash
docker run -d -p 9000:8080 atendi-tickets-api
# Accede a http://localhost:9000
```

### Ver logs del contenedor
```bash
docker-compose logs -f
# O
docker logs atendi-api
```

### Ver contenedores en ejecución
```bash
docker ps
```

### Eliminar la base de datos (reiniciar desde cero)
```bash
docker-compose down -v
```

## 📦 Estructura de archivos

```
AtiendiTicketsAPI/
├── Dockerfile              # Configuración de Docker
├── docker-compose.yml      # Orquestación de contenedor
├── .dockerignore          # Archivos a excluir
├── appsettings.json       # Configuración actualizada para Docker
├── Program.cs
├── Controllers/
│   └── TicketController.cs
├── Datos/
│   └── ApplicationDbContext.cs
├── Entidades/
│   └── Ticket.cs
├── Enum/
│   ├── Prioridad.cs
│   └── Estado.cs
└── Migrations/
```

## ✅ Verificar que todo funciona

1. Abre el navegador en: `http://localhost:8080/swagger/ui/index.html`
2. Deberías ver la interfaz de Swagger
3. Prueba crear un ticket desde la UI
4. Verifica que lo puedas recuperar

¡Listo! Tu maestro solo necesita ejecutar `docker-compose up --build` 🎉
