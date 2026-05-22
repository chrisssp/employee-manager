<h1 align="center"> Employee Manager — Employee & Role Management System</h1>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-8-512BD4?logo=dotnet&logoColor=white" alt=".NET 8">
  <img src="https://img.shields.io/badge/Next.js-16-000000?logo=next.js&logoColor=white" alt="Next.js">
  <img src="https://img.shields.io/badge/React-19-61DAFB?logo=react&logoColor=white" alt="React">
  <img src="https://img.shields.io/badge/TypeScript-5-3178C6?logo=typescript&logoColor=white" alt="TypeScript">
  <img src="https://img.shields.io/badge/SQL_Server-CC2927?logo=microsoft-sql-server&logoColor=white" alt="SQL Server">
</p>

<p align="center">
  <em>Full-stack monorepo for employee and role management with CRUD operations, pagination, and audit logging.</em>
</p>

<p align="center">
  <a href="README.md">🇬🇧 English</a> · <a href="README.es.md">🇪🇸 Español</a>
</p>

---

## About Employee Manager

A full-stack monorepo featuring an ASP.NET Core 8 backend and a Next.js 16 frontend for managing employees, roles, and audit logs with server-side pagination and soft delete support.

## Project Structure

```
employee-manager/
├── api/                      # Backend (ASP.NET Core 8)
│   ├── Controllers/         # HTTP endpoints
│   ├── Services/            # Business logic
│   ├── Models/              # Database entities
│   ├── DTOs/                # Transfer models
│   ├── Interfaces/          # Service contracts
│   ├── Middlewares/         # Custom middleware
│   ├── Migrations/          # EF Core migrations
│   └── Program.cs           # Main configuration
│
├── frontend/                 # Frontend (Next.js 16 + React 19)
│   ├── src/
│   │   ├── app/            # Routes and pages
│   │   ├── components/     # React components
│   │   ├── services/       # API clients
│   │   └── lib/            # Utilities
│   └── package.json        # Dependencies
│
├── .gitignore
├── LICENSE
└── README.md
```

## Quick Start

### Backend

```bash
cd api
dotnet restore
dotnet build
dotnet run
```

Backend available at: `http://localhost:5110`

### Frontend

```bash
cd frontend
npm install
npm run dev
```

Frontend available at: `http://localhost:3000`

## Database

- **Engine:** SQL Server
- **Database:** `Empresa`
- **Server:** localhost (local development)

Migrations are applied automatically on startup.

## Features

### Employees
- Full CRUD (Create, Read, Update, Delete)
- Role assignment
- Soft delete and reactivation
- Server-side pagination (10 records/page)

### Roles
- Full CRUD
- Validation (cannot delete roles with assigned employees)
- Server-side pagination

### Logs
- Automatic HTTP transaction logging
- Payload capture
- Timestamp with timezone
- Server-side pagination

## Tech Stack

### Backend
- .NET 8
- Entity Framework Core
- SQL Server
- ASP.NET Core
- Exception and logging middleware

### Frontend
- Next.js 16.2.4
- React 19
- TypeScript
- React Hook Form
- Axios
- Tailwind CSS
- shadcn/ui

## API Endpoints

### Employees
- `GET /api/empleados` - List (with pagination)
- `GET /api/empleados/{id}` - Get by ID
- `POST /api/empleados` - Create
- `PUT /api/empleados/{id}` - Update
- `DELETE /api/empleados/{id}` - Soft delete
- `POST /api/empleados/{id}/reactivar` - Reactivate

### Roles
- `GET /api/roles` - List (with pagination)
- `GET /api/roles/{id}` - Get by ID
- `POST /api/roles` - Create
- `PUT /api/roles/{id}` - Update
- `DELETE /api/roles/{id}` - Delete

### Logs
- `GET /api/logs` - List (with pagination)
- `GET /api/logs/{id}` - Get by ID

## Contributing

Please read [CONTRIBUTING.md](CONTRIBUTING.md) for branch naming, commit conventions, and PR workflow.

## License

This project is licensed under the GPL v3 — see the [LICENSE](LICENSE) file for details.

## Acknowledgments

**Authors:**

- [@chrisssp](https://github.com/chrisssp) — Christian Serrano
