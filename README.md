# 📌 Project & Task Management API

A scalable backend system for managing projects and tasks. Each authenticated user can create, update, and manage **their own projects and tasks** using a secure JWT-based API.

---

## 🔎 Project Overview
This API provides a production-ready backend for project and task management with **Clean Architecture**, **JWT Authentication**, and **global exception handling**. It is designed to be modular, testable, and easy to extend.

---

## ✅ Features
### 🔐 Authentication
- Register
- Login with JWT

### 📁 Projects
- Create Project (authenticated)
- Get All Projects (user scoped)
- Get Project By Id (user scoped)
- Update Project (user scoped)
- Delete Project (user scoped)

### ✅ Tasks
- Create Task (under user project)
- Get All Tasks (user scoped)
- Get Task By Id (user scoped)
- Update Task (user scoped)
- Update Task Status
- Get Tasks By Project
- Delete Task (user scoped)

---

## 🧱 Architecture Overview
This solution follows **Clean Architecture** with clear separation of concerns.

**Layers:**
- **API** → Controllers, Middleware, Swagger
- **Application** → Services, DTOs, Business Logic
- **Domain** → Entities, Contracts, DTOs
- **Infrastructure** → EF Core, Repositories, DbContext

---

## 🛠️ Technologies Used
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- Clean Architecture
- FluentValidation
- Global Exception Handling
- Swagger / OpenAPI

---

## 🗂️ Project Structure
```
TaskFlowSolution/
├─ TaskFlow.API/             # API layer (Controllers, Middleware)
├─ TaskFlow.Application/     # Application services & logic
├─ TaskFlowDomain/           # Domain entities, DTOs, contracts
├─ TaskFlow.InfraStructure/  # EF Core, repositories, db context
```

---

## 🗃️ Database Configuration
Update your SQL Server connection string in **appsettings.json**:

```json
"connectionStrings": {
  "DefaultConnection": "Server=.;Database=TaskFlowDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

---

## 🔐 JWT Configuration
Configure JWT settings in **appsettings.json**:

```json
"JWTSetting": {
  "Key": "This_Is_Secret_Key_For_Token_Generation",
  "issuer": "TalanatApi",
  "audience": "TalanatClient",
  "DurationInDays": 7
}
```

---

## ▶️ How to Run the Project
### ✅ Prerequisites
- .NET 10 SDK
- SQL Server

### ✅ Steps
```bash
# Restore packages
dotnet restore

# Apply migrations
dotnet ef database update --project TaskFlow.InfraStructure --startup-project TaskFlow.API

# Run the API
dotnet run --project TaskFlow.API
```

---

## 🧬 Migration Commands
```bash
# Add migration
dotnet ef migrations add InitialMigration --project TaskFlow.InfraStructure --startup-project TaskFlow.API

# Update database
dotnet ef database update --project TaskFlow.InfraStructure --startup-project TaskFlow.API
```

---

## 📡 API Endpoints
### 🔐 Authentication
| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | /api/Account/register | Register new user |
| POST | /api/Account/login | Login and get JWT |

### 📁 Projects
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/Project | Get all projects (user scoped) |
| GET | /api/Project/{id} | Get project by id (user scoped) |
| POST | /api/Project | Create project |
| PUT | /api/Project/{id} | Update project |
| DELETE | /api/Project/{id} | Delete project |

### ✅ Tasks
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/Task | Get all tasks (user scoped) |
| GET | /api/Task/{id} | Get task by id (user scoped) |
| GET | /api/Task/project/{projectId} | Get tasks by project |
| POST | /api/Task | Create task |
| PUT | /api/Task/{id} | Update task |
| PUT | /api/Task/{id}/status | Update task status |
| DELETE | /api/Task/{id} | Delete task |

---

## 🧪 Swagger Usage
Swagger UI is enabled in development.

Open:
```
https://localhost:{PORT}/swagger
```
Use **Authorize** button to set your JWT token.

---

## ✅ Validation & Exception Handling
- Input validation with **FluentValidation**
- Global exception middleware returns consistent API responses
- Common errors return:
  - **400** Bad Request
  - **401** Unauthorized
  - **404** Not Found

---

## 🚀 Future Improvements
- Refresh Tokens
- Pagination & Filtering
- Role-based authorization
- Unit & integration tests
- Docker support

---

## 👨‍💻 Author
**Backend .NET Developer Technical Assessment**

If you need any adjustments or enhancements, feel free to reach out.
