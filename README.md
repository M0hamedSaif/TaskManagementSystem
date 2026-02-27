# 🚀 Task Management System

A RESTful API built with **ASP.NET Core .NET 8** following **Clean (Onion) Architecture**.  
Developed by **Mohamed Saif**.

---

## 📑 Table of Contents
- [Project Structure](#-project-structure)
- [Architecture Overview](#-architecture-overview)
- [Technologies Used](#️-technologies-used)
- [Database Setup](#-database-setup)
- [Sample Test Users](#-sample-test-users)
- [API Endpoints](#-api-endpoints)
- [Running the Project](#️-running-the-project)
- [Testing](#-testing)
- [Additional Notes](#-additional-notes)

---

## 🏗 Project Structure
```
TaskManager/
 ├── TaskManager.Core/        # Domain layer
 │    ├── Entities/
 │    ├── Repositories.Contract/
 │    ├── Services.Contract/
 │    ├── Specifications/
 │    └── IUnitOfWork.cs
 │
 ├── TaskManager.Repository/  # Data access layer
 │    ├── Data/
 │    ├── Configurations/
 │    ├── Context/
 │    ├── Migrations/
 │    └── UnitOfWork/
 │
 ├── TaskManager.Service/     # Business logic layer
 │    └── Services/
 │
 └── TaskManager.APIs/        # Presentation layer
      ├── Controllers/
      ├── Dto/
      ├── Errors/
      ├── Extensions/
      ├── Helpers/
      └── Middlewares/
```

---

## 🧅 Architecture Overview
This project follows **Onion Architecture** with 4 layers:

| Layer      | Project                | Responsibility                          |
|------------|------------------------|-----------------------------------------|
| Core       | TaskManager.Core       | Entities, interfaces, specifications    |
| Repository | TaskManager.Repository | EF Core, migrations, data access        |
| Service    | TaskManager.Service    | Business logic, JWT, calculations       |
| API        | TaskManager.APIs       | Controllers, DTOs, middleware           |

**Key Patterns Used:**  
✔ Repository Pattern  
✔ Unit of Work Pattern  
✔ Specification Pattern  
✔ Dependency Injection  

---

## ⚙️ Technologies Used
| Technology              | Version   | Purpose                          |
|--------------------------|-----------|----------------------------------|
| ASP.NET Core             | .NET 8    | Web API framework                |
| Entity Framework Core    | 8.0.24    | ORM + Code First migrations      |
| SQL Server               | Local     | Database                         |
| ASP.NET Core Identity    | 8.0.24    | User management + roles          |
| JWT Bearer               | 8.0.24    | Authentication tokens            |
| AutoMapper               | 16.0.0    | DTO ↔ Entity mapping             |
| Swashbuckle (Swagger)    | 6.6.2     | API documentation                |

---

## 🗄 Database Setup
**Prerequisites:**  
- SQL Server installed locally  
- .NET 8 SDK  
- Visual Studio 2022  

**Steps:**  
1. Clone the repository  
   ```bash
   git clone https://github.com/M0hamedSaif/TaskManagementSystem
   cd Task-Management-System
   ```
2. Update connection string in `TaskManager.APIs/appsettings.json`  
3. Run the project → migrations & seeding happen automatically  
---

## 🧪 Testing
- Postman collection included: `Data/Postman/TaskManager API.postman_collection.json`  
- Covers: Auth, Tasks, Reminders, Dashboard, Pricing  

---


## 👥 Sample Test Users
**Default Admin (Seeded):**
- Email: `admin@taskmanager.com`  
- Password: `Admin@123456`  
- Role: `Admin`

**Create via Register Endpoint:**
```json
{
  "displayName": "Leader Ahmed",
  "email": "leader@taskmanager.com",
  "password": "Leader@123456",
  "role": "TeamLeader",
  "team": "Design"
}
```

```json
{
  "displayName": "Member Mohamed",
  "email": "member@taskmanager.com",
  "password": "Member@123456",
  "role": "TeamMember",
  "team": "Design"
}
```

---

## 🔗 API Endpoints

### Authentication
| Method | Endpoint             | Auth            | Description        |
|--------|----------------------|-----------------|--------------------|
| POST   | `/api/account/login` | Public          | Login + JWT token  |
| POST   | `/api/account/register` | Admin only   | Create new user    |
| GET    | `/api/account/users` | Admin/Leader    | List users         |

### Tasks
| Method | Endpoint                  | Auth                  | Description          |
|--------|---------------------------|-----------------------|----------------------|
| POST   | `/api/tasks`              | TeamLeader            | Create task          |
| PUT    | `/api/tasks/{id}`         | TeamLeader (creator)  | Update task          |
| PATCH  | `/api/tasks/{id}/status`  | TeamMember (assignee) | Update status        |
| GET    | `/api/tasks/my-tasks`     | TeamMember            | Get my tasks         |
| GET    | `/api/tasks/my-team-tasks`| TeamLeader            | Get team tasks       |
| GET    | `/api/tasks/upcoming-deadlines` | Any auth        | Tasks due in 24h     |
| GET    | `/api/tasks/{id}`         | Any auth              | Get task by ID       |

### Dashboard
| Method | Endpoint                 | Auth             | Description        |
|--------|--------------------------|------------------|--------------------|
| GET    | `/api/dashboard/summary` | Admin/TeamLeader | Full statistics    |

### Projects / Pricing
| Method | Endpoint                  | Auth             | Description        |
|--------|---------------------------|------------------|--------------------|
| POST   | `/api/projects`           | Admin/TeamLeader | Create project     |
| PUT    | `/api/projects/{id}/costs`| Admin/TeamLeader | Update costs       |
| PUT    | `/api/projects/{id}/send` | Admin/TeamLeader | Mark as Sent       |

---

## ▶️ Running the Project
1. Open `TaskManager.sln` in Visual Studio 2022  
2. Set **TaskManager.APIs** as startup project  
3. Press **F5** → Swagger UI opens at `https://localhost:7162/swagger`  
4. Use **Authorize** button → `Bearer <your-token>`  


---

## 📌 Additional Notes
- Enums sent/received as strings (e.g., "Design")  
- DateTime stored in UTC  
- Consistent error response format:  
  ```json
  {
    "statusCode": 404,
    "message": "Not Found"
  }
  ```
- JWT tokens expire after 7 days
