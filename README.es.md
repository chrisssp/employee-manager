<h1 align="center"> Employee Manager — Sistema de Gestión de Empleados y Roles</h1>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-8-512BD4?logo=dotnet&logoColor=white" alt=".NET 8">
  <img src="https://img.shields.io/badge/Next.js-16-000000?logo=next.js&logoColor=white" alt="Next.js">
  <img src="https://img.shields.io/badge/React-19-61DAFB?logo=react&logoColor=white" alt="React">
  <img src="https://img.shields.io/badge/TypeScript-5-3178C6?logo=typescript&logoColor=white" alt="TypeScript">
  <img src="https://img.shields.io/badge/SQL_Server-CC2927?logo=microsoft-sql-server&logoColor=white" alt="SQL Server">
</p>

<p align="center">
  <em>Monorepo full-stack para gestión de empleados y roles con operaciones CRUD, paginación y registro de auditoría.</em>
</p>

<p align="center">
  <a href="README.md">🇬🇧 English</a> · <a href="README.es.md">🇪🇸 Español</a>
</p>

---

## Acerca de Employee Manager

Un monorepo full-stack con backend ASP.NET Core 8 y frontend Next.js 16 para gestionar empleados, roles y registros de auditoría con paginación del lado del servidor y soporte de borrado lógico.

## Estructura del proyecto

```
employee-manager/
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
├── .gitignore
├── LICENSE
└── README.md
```

## Inicio rápido

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

## Base de Datos

- **Motor:** SQL Server
- **Base de datos:** `Empresa`
- **Servidor:** localhost (local development)

Migraciones automáticas se aplican al iniciar la aplicación.

## Funcionalidades

### Empleados
- CRUD completo (Crear, Leer, Actualizar, Eliminar)
- Asignación de roles
- Soft delete (baja lógica) y reactivación
- Paginación server-side (10 registros/página)

### Roles
- CRUD completo
- Validación de eliminación (no permite si hay empleados asignados)
- Paginación server-side

### Logs
- Registro automático de transacciones HTTP
- Captura de payload
- Timestamp con timezone
- Paginación server-side

## Stack Tecnológico

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

## API Endpoints

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

## Contribuciones

Lee [CONTRIBUTING.md](CONTRIBUTING.md) para conocer las convenciones de ramas, commits y PRs.

## Licencia

Este proyecto está bajo la licencia GPL v3 — ver [LICENSE](LICENSE) para más detalles.

## Agradecimientos

**Authors:**

- [@chrisssp](https://github.com/chrisssp) — Christian Serrano
