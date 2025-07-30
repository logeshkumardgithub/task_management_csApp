
# 📝 Task Management Application

This is a .NET 8 based **Task Management System**.
It contains:
- A RESTful Web API (`TaskManagement.API`) to manage tasks
- A Razor Pages UI (`TaskManagement.UI`) to interact with the API
- Basic authentication for securing the API endpoints

---

## ✅ Features Implemented

- Task operations via REST API
- Razor Pages UI with:
  - Task listing with filters and search
  - Create, Edit, Delete, and View task details
  - Client-side validation using jQuery
- Bootstrap UI styling
- Swagger integration for API testing
- Basic Authentication enabled for API access
- Search & Filter tasks by `title` and `status`

---

## 🛠️ Tools & Versions

| Tool                     | Version       |
|--------------------------|---------------|
| .NET SDK                 | **8.0**       |
| Visual Studio Code       | Recommended   |
| Razor Pages              | Built-in      |
| ASP.NET Core Web API     | .NET 8        |
| Swagger (Swashbuckle)    | v6+           |
| Bootstrap                | v5+           |
| jQuery Validation        | Latest        |

---

## 🧱 Project Structure

```
TaskManagementApp/
│
├── TaskManagement.API         // Web API project
│   ├── Controllers
│   ├── Models
│   ├── Data
│   ├── Authentication         // Basic Auth Handler
│   ├── Program.cs
│
├── TaskManagement.UI          // Razor UI project
│   ├── Pages/Tasks
│   │   ├── Index.cshtml       // Task List
│   │   ├── Create.cshtml      // Add Task
│   │   ├── Edit.cshtml        // Update Task
│   │   ├── Details.cshtml     // Task Info
│   ├── wwwroot                // Bootstrap, JS, CSS
│   ├── _Layout.cshtml
│   ├── _ViewImports.cshtml
│   ├── _ValidationScriptsPartial.cshtml
│
└── TaskManagementApp.sln      // Solution file
```

---

## 🚀 How to Run the Application

### 📦 Prerequisites

- Install [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- Visual Studio Code or any C# IDE
- Command Line / Terminal

---

### 🔧 Setup

1. Clone or extract the project to your system
	[git clone git@github.com:logeshkumardgithub/task_management_csApp.git]
2. Open the project in **Visual Studio Code** using [TaskManagementApp.sln] file
3. Open terminal and navigate to root directory

---

### 📁 Restore & Build

```bash
dotnet restore
dotnet build
```

---

### ▶️ Run the Web API

```bash
cd TaskManagement.API
dotnet run
```

- Swagger will be available at:
  http://localhost:5000/swagger
- You’ll be prompted for basic auth credentials

---

### ▶️ Run the Razor UI

In a new terminal:

```bash
cd TaskManagement.UI
dotnet run
```

- UI will be available at:
  http://localhost:5001

---

## 👨‍💻 Author

Logeshkumar Dharmalingam
