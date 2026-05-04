# EmpresaPrueba - Monorepo

Sistema de gestión de empleados y roles con backend .NET y frontend Next.js.

## 📁 Estructura

```
EmpresaPrueba/
├── api/                      # Backend (ASP.NET Core 8)
│   ├── Controllers/         # Endpoints HTTP
│   ├── Services/            # Lógica de negocio
│   ├── Models/              # Entidades de BD
│   ├── DTOs/                # Modelos de transferencia
│   ├── Interfaces/          # Contratos de servicios
│   ├── Middlewares/         # Middlewares custom
│   ├── Migrations/          # Migraciones EF Core
│   └── Program.cs           # Configuración principal
│
├── frontend/                 # Frontend (Next.js 16 + React 19)
│   ├── src/
│   │   ├── app/            # Rutas y páginas
│   │   ├── components/     # Componentes React
│   │   ├── services/       # Clientes API
│   │   └── lib/            # Utilidades
│   └── package.json        # Dependencias
│
├── .gitignore              # Excepciones de git
└── README.md               # Este archivo
```

## 🚀 Quick Start

### Backend

```bash
cd api
dotnet restore
dotnet build
dotnet run
```

Backend disponible en: `http://localhost:5110`

### Frontend

```bash
cd frontend
npm install
npm run dev
```

Frontend disponible en: `http://localhost:3000`

## 🗄️ Base de Datos

- **Motor:** SQL Server
- **Base de datos:** `Empresa`
- **Servidor:** localhost (local development)

Migraciones automáticas se aplican al iniciar la aplicación.

## 📋 Funcionalidades

### Empleados
- ✅ CRUD completo (Crear, Leer, Actualizar, Eliminar)
- ✅ Asignación de roles
- ✅ Soft delete (baja lógica) y reactivación
- ✅ Paginación server-side (10 registros/página)

### Roles
- ✅ CRUD completo
- ✅ Validación de eliminación (no permite si hay empleados asignados)
- ✅ Paginación server-side

### Logs
- ✅ Registro automático de transacciones HTTP
- ✅ Captura de payload
- ✅ Timestamp con timezone
- ✅ Paginación server-side

## 🛠️ Stack Tecnológico

### Backend
- .NET 8
- Entity Framework Core
- SQL Server
- ASP.NET Core
- Middleware de excepciones y logging

### Frontend
- Next.js 16.2.4
- React 19
- TypeScript
- React Hook Form
- Axios
- Tailwind CSS
- shadcn/ui

## 📝 API Endpoints

### Empleados
- `GET /api/empleados` - Listar (con paginación)
- `GET /api/empleados/{id}` - Obtener por ID
- `POST /api/empleados` - Crear
- `PUT /api/empleados/{id}` - Actualizar
- `DELETE /api/empleados/{id}` - Baja lógica
- `POST /api/empleados/{id}/reactivar` - Reactivar

### Roles
- `GET /api/roles` - Listar (con paginación)
- `GET /api/roles/{id}` - Obtener por ID
- `POST /api/roles` - Crear
- `PUT /api/roles/{id}` - Actualizar
- `DELETE /api/roles/{id}` - Eliminar

### Logs
- `GET /api/logs` - Listar (con paginación)
- `GET /api/logs/{id}` - Obtener por ID

## 👤 Autor

Christian Serrano Puertos (chrisssp)

---

**Última actualización:** Mayo 2026
