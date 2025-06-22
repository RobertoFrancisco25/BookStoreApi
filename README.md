# 📚 BookStoreApi

**BookStoreApi** is a REST API built with **.NET 8** that manages **books** and **categories** for an online bookstore.  
It supports CRUD operations, pagination, filtering, and is ready for containerized deployment with Docker.

> All endpoints are prefixed with `/api`.

---

## 🚀 Technologies Used

- [.NET 8](https://dotnet.microsoft.com/)
- [Docker](https://www.docker.com/)
- [JetBrains Rider](https://www.jetbrains.com/rider/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- C#
- Windows 10

---

## 🛠️ Getting Started

### ✅ Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- Git

---

### 📥 Clone the repository

```bash
git clone git@github.com:RobertoFrancisco25/BookStoreApi.git
cd BookStoreApi
```
---

## ▶️ How to Run the Project

### 🐳 Using Docker
```bash
docker build -t bookstoreapi .
docker run -d -p 5000:80 --name bookstoreapi-container bookstoreapi
```
Access the API at: http://localhost:5000/api
---
### ⚙️ Running Locally with .NET
```bash
dotnet restore
dotnet build
dotnet run --project src/BookStoreApi
```
Access the API at:
https://localhost:5001/api
or http://localhost:5000/api
---
📬 API Endpoints
Base path: /api
🔎 You can test the endpoints using Swagger or Postman.
---
🧪 Running Tests
```bash
dotnet test
```
