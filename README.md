# 💼 Payroll Management System

A web-based **Payroll Management System** built using **ASP.NET Core Web API, Entity Framework Core, Angular, and SQL Server** to manage employees, salaries and leave requests with role-based access control.

## 🛠️ Tech Stack

- **Backend:** ASP.NET Core Web API (.NET 8 / .NET 9)
- **Frontend:** Angular
- **Database:** SQL Server
- **ORM:** Entity Framework Core
- **Authentication:** JWT and Refresh Tokens
- **Validation:** FluentValidation
- **API Documentation:** Swagger / OpenAPI

## ✨ Features

### 👨‍💼 Employee Management
- Create, view, update, and manage employee records.
- Assign reporting managers to employees.
- View employee profiles based on user permissions.

### 💰 Payroll Management
- Maintain employee salary structures.
- Manage allowances and deductions.
- Calculate net salaries.
- View salary information based on assigned roles.

### 🏖️ Leave Management
- Apply for leave and track request status.
- Enable managers to approve or reject team leave requests.
- Allow admins to manage manager leave requests.
- Prevent unauthorized approvals and self-approval.

### 🔐 Role-Based Access Control
- Separate permissions for Admin, Manager, and Employee.
- Restrict access to employee, salary, and leave records.
- Secure API endpoints based on user roles.

### 🔑 Secure Authentication
- JWT-based authentication.
- Refresh token support.
- Role-based authorization.
- Protected API endpoints.

### ⚙️ Data Handling
- Pagination, filtering, and sorting.
- Request validation using FluentValidation.
- Centralized exception handling.
- Swagger / OpenAPI documentation.

## 👥 User Roles and Permissions

| Feature | Admin | Manager | Employee |
|---|---|---|---|
| Manage employees | ✅ | ❌ | ❌ |
| Manage salary structures | ✅ | ❌ | ❌ |
| View all employee records | ✅ | ❌ | ❌ |
| View team members | ✅ | ✅ | ❌ |
| View own profile | ✅ | ✅ | ✅ |
| View own salary | ✅ | ✅ | ✅ |
| Apply for leave | ✅ | ✅ | ✅ |
| Approve team leave | ❌ | ✅ | ❌ |
| Approve manager leave | ✅ | ❌ | ❌ |
| Track leave status | ✅ | ✅ | ✅ |

## 🏗️ Architecture

The application follows a layered architecture to separate business logic and data access.

```text
Angular Frontend
       ↓
ASP.NET Core Web API
       ↓
   Controllers
       ↓
    Services
       ↓
   Repository
       ↓
Entity Framework Core
       ↓
   SQL Server
```

## 🔄 Leave Approval Workflow

1. Employees submit leave requests to their assigned managers.
2. Managers review and approve or reject team leave requests.
3. Managers submit their own leave requests to the Admin.
4. Admins approve or reject manager leave requests.
5. Users can track the status of their leave requests.
6. Only authorized users can approve or reject requests.
7. Self-approval is not allowed.

## 🚀 Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Node.js and npm](https://nodejs.org/)
- [Angular CLI](https://angular.dev/tools/cli)
- [SQL Server](https://www.microsoft.com/en-us/sql-server)

### 1. Clone the Repository

```bash
git clone https://github.com/Ritu-Jangra/Payroll-Management_EF.git
cd Payroll-Management_EF
```

### 2. Configure the Database

Update the SQL Server connection string in `appsettings.json` or `appsettings.Development.json`.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=PayrollManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Replace `YOUR_SERVER_NAME` with your SQL Server instance name.

### 3. Run the Backend

Navigate to the directory containing the backend `.csproj` file.

```bash
dotnet restore
dotnet build
dotnet run
```

If the project uses Entity Framework Core migrations, run:

```bash
dotnet ef database update
```

### 4. Run the Angular Frontend

Navigate to the directory containing `package.json`.

```bash
npm install
ng serve
```

Open the local URL displayed in the terminal, typically `http://localhost:4200`.

**Note:** Adjust commands and directory paths according to the actual project structure.

## 🔌 API Documentation

Swagger / OpenAPI can be used to explore and test API endpoints when configured.

After starting the backend, open the Swagger URL configured in the application, typically:

`https://localhost:<port>/swagger`

Use the actual port displayed by your application.

## 🔒 Security Considerations

- Protect API endpoints using JWT authentication.
- Enforce role-based authorization on the backend.
- Secure refresh tokens and implement expiration and revocation.
- Validate incoming requests.
- Prevent unauthorized access to salary and employee records.
- Enforce leave approval rules on the server.
- Store secrets outside source control.
- Use HTTPS in production.

## 👩‍💻 Author

**Ritu Jangra**

- GitHub: [@Ritu-Jangra](https://github.com/Ritu-Jangra)
- Repository: [Payroll-Management_EF](https://github.com/Ritu-Jangra/Payroll-Management_EF)

## 🤝 Contributing

Contributions and suggestions are welcome.

1. Fork the repository.
2. Create a feature branch.
3. Make your changes.
4. Commit and push your changes.
5. Open a pull request.

## 📄 License

No license information is specified. Add a `LICENSE` file if you intend to distribute the project under an open-source license.

## ⭐ Support

If you find this project useful, consider giving the repository a star on GitHub!
