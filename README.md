💼 Payroll Management System

A web-based Payroll Management System built using ASP.NET Core Web API, Entity Framework Core, Angular, and SQL Server to manage employees, salaries, payslips, and leave requests with role-based access control.

🛠️ Tech Stack
Backend: ASP.NET Core Web API (.NET 8/9)
Frontend: Angular
Database: SQL Server
ORM: Entity Framework Core
Authentication: JWT and Refresh Tokens
Validation: FluentValidation
API Documentation: Swagger / OpenAPI
✨ Features
Employee Management: Manage employee records and assign reporting managers.
Payroll Management: Maintain salary structures, allowances, deductions, and net salary.
Leave Management: Apply for leave, track requests, and manage approvals.
Role-Based Access: Separate permissions for Admin, Manager, and Employee.
Secure Authentication: JWT-based authentication with refresh tokens.
Data Handling: Pagination, filtering, sorting, and centralized exception handling.
👥 User Roles
Admin	Manager	Employee
Manage employees	View team members	View own profile
Manage salaries	View own salary	View own salary
Approve manager leave	Approve team leave	Apply for leave
View all records	Apply for leave	Track leave status
🏗️ Architecture

The application follows a layered architecture to separate business logic and data access.

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

🔄 Leave Approval Workflow
Employees submit leave requests to their assigned managers.
Managers submit leave requests to the Admin.
Only authorized users can approve or reject requests.
Self-approval is not allowed.
🚀 Getting Started
Prerequisites
.NET SDK
Node.js
Angular CLI
SQL Server
Installation

1. Clone the repository

git clone https://github.com/Ritu-Jangra/Payroll-Management_EF.git
cd Payroll-Management_EF


2. Configure the database

Update the SQL Server connection string in the backend configuration.

3. Run the backend

dotnet restore
dotnet run


4. Run the Angular frontend

Navigate to the frontend project directory and execute:

npm install
ng serve

Adjust the commands according to the project's actual folder structure and configuration.
🔌 API Documentation

Swagger / OpenAPI can be used to explore and test the API endpoints when configured.

👩‍💻 Author

Ritu Jangra

GitHub: @Ritu-Jangra

⭐ If you find this project useful, consider giving the repository a star!
