ASP.NET Core Web API Project - User Management API

Overview

This project is a backend application developed using ASP.NET Core Web API and Entity Framework with a code-first approach. It is designed to manage user data efficiently, providing endpoints for user authentication, data retrieval, and data management.

Features

Login API

Endpoint to authenticate users using a username and password.

User List API

Endpoint to retrieve a list of users with details:

Photo

Name

Email

Mobile Number

Manual User Addition API

Endpoint to add users by sending their information via POST requests.

Excel Import API

Endpoint to upload an Excel file containing user data.

Automatically generates passwords for imported users.

Excludes image data during the import process.

Optimized to handle one million records and complete the import within two minutes.

Technologies Used

Backend: ASP.NET Core Web API

Database: SQL Server

ORM: Entity Framework (Code-First Approach)

Installation

Prerequisites

.NET SDK installed

SQL Server installed

A tool like Postman or Swagger for API testing

Steps

Clone the repository:

git clone https://github.com/your-username/your-repository.git

Navigate to the project directory:

cd your-project-directory

Update the database connection string in appsettings.json:

"ConnectionStrings": {
    "DefaultConnection": "Server=your-server;Database=your-database;User Id=your-username;Password=your-password;"
}

Apply migrations to set up the database:

dotnet ef database update

Build and run the application:

dotnet run

Access the API documentation (Swagger) in your browser:

http://localhost:5000/swagger

API Endpoints

Authentication

POST /api/auth/login

Request body:

{
    "username": "example",
    "password": "example"
}

User Management

GET /api/users

Response body:

[
    {
        "photo": "url-to-photo",
        "name": "John Doe",
        "email": "john.doe@example.com",
        "mobileNumber": "123456789"
    }
]

POST /api/users

Request body:

{
    "name": "John Doe",
    "email": "john.doe@example.com",
    "mobileNumber": "123456789",
    "password": "example"
}

Excel Import

POST /api/users/import

Request: Upload Excel file.

Performance Optimization for Excel Import

Bulk insert operations are used to minimize database interaction.

File validation and processing are handled asynchronously.

Folder Structure

![image](https://github.com/user-attachments/assets/12d9c0a4-c475-465f-b3a1-2600e59a78d2)

Contributing

Fork the repository.

Create a feature branch:

git checkout -b feature-name

Commit your changes:

git commit -m "Add new feature"

Push to the branch:

git push origin feature-name

Create a pull request.



