# AtiendiTicketsAPI - Docker Setup

Esta guía explica cómo ejecutar la API de tickets usando Docker.

## 📋 Requisitos previos

- Docker instalado ([Descargar Docker Desktop](https://www.docker.com/products/docker-desktop))
- Docker Compose (incluido en Docker Desktop)

## 🚀 Método de ejecución

### Usando Docker Compose (RECOMENDADO)

```bash
# Clonar o descargar el repositorio
git clone https://github.com/AbrahamDev950/TicketsAPI.git

# Accede al repositorio
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

## 📍 Acceder a la API

Una vez que el contenedor esté corriendo:

- **Swagger UI:** http://localhost:8080/swagger
- **API Base:** http://localhost:8080/api/tickets

## 📝 Ejemplo de uso en terminal

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

Los datos **persisten** incluso si paras y reinicias el contenedor.

## 🐛 Solución de problemas

### "Puerto 8080 ya está en uso"
Usar otro puerto:
```bash
docker run -d -p 9000:8080 atendi-tickets-api
# Accede a http://localhost:9000
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
├── Dockerfile              
├── docker-compose.yml      
├── .dockerignore          
├── appsettings.json       
├── Program.cs
├── data/
│   └── AtiendiTicketsDB.db
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

1. Abre el navegador en: `http://localhost:8080/swagger`
2. Deberías ver la interfaz de Swagger
3. Prueba crear un ticket desde la UI
4. Verifica que lo puedas recuperar

