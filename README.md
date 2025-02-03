# PeerEvalApp
An efficient platform for conducting and managing peer and supervisor evaluations within companies, structured around workgroups

## Table of Contents
- [About The Project](#about-the-project)
- [Features](#features)
- [Built With](#built-with)
- [Documentation](#documentation)
- [Start Up](#start-up)
- [Prerequisites](#prerequisites)

## About The Project
PeerEvalApp was created as the final assignment for [Coding Factory](https://codingfactory.aueb.gr/). It is a Full Stack App that allows employees of a company to evaluate each other. Each employee can have a record of their evaluation grades from completed evaluation cycles. The project provided an excellent opportunity to explore the Model-First development pattern in the backend using .NET and to gain practical experience with Angular in the frontend. This was my first approach to web development in a personal project context, expanding my skills in both designing and implementing functional user interfaces and robust backend solutions.

## Features
- **User Management**  
  - Create and update users (Admin vs. User roles).
- **Evaluation Cycles**  
  - Admins open/close cycles; Users view active cycles and submit peer evaluations.
- **Peer Evaluations**  
  - Rate colleagues within the same team; track completed evaluations.
- **Historical Data**  
  - View past cycles, results, and performance over time.
- **Auth & Authorization**  
  - Distinguish Admin powers (managing data) from User capabilities (evaluations).
- **Extensibility**  
  - Layered architecture (Controllers, Services, Repos) for easy feature addition.

## Built With
- **Database:**
  - Microsoft SQL Server
- **API:**
  - .NET 8.0 & C# 12
- **WebApp:**
  - Angular 18.2.13

## Documentation
- **DB Schema:** The database schema is available [here](./DiagramsAndDocumentation/PeerEvalAppDbSchema.svg). For an interactive version, visit [dbdocs.io](https://dbdocs.io/konbarbou/PeerEvalAppDbSchema).
- **Class Diagram:** Visualize the application's architecture [here](./DiagramsAndDocumentation/PeerEvalAppClassDiagram.svg).
- **Role-Based Overview:** Detailed workflows based on user roles can be checked [here](./DiagramsAndDocumentation/NotesWorkFlows.md).
- **Endpoint Overview:** For a detailed API endpoint overview similar to Swagger, go [here](./DiagramsAndDocumentation/EndpointOverview.md).

## Prerequisites
- SQL Server & SSMS
- .NET SDK (8.0 or higher)
- Node.js and npm
- Angular CLI

## Start Up

### Backend Configurations

1. **Create Database:**
   - Open **SSMS** and create a new database named `PeerEvalDB`.
   - Set up a Login as the owner of the database.

2. **Update Connection String:**
   - Open `PeerEvalAppAPI/appsettings.json`.
   - In the `ConnectionStrings` section, update the `DefaultConnection` with your SQL Server username (the Login you set up as the DB owner) and the corresponding password.

3. **Update Database Schema:**
   - Open the **Package Manager Console**.
   - Run the command:
     ```powershell
     Update-Database
     ```
   - *Note:* If no migrations are detected, first run:
     ```powershell
     Add-Migration YourMigrationName
     ```
     then run:
     ```powershell
     Update-Database
     ```
4. **Populate Tables**
   - Run the [SQL scripts](./PeerEvalApp/SQL_Scripts) which:
     - Populate the Questions Table.
     - Add an Admins Group
     - Add an Admin User with username: **admin@example.com** and password: **Super111!!!**
4. **Run the Backend:**
   - Start the project.
   - Verify that everything is set up correctly by checking if the Swagger UI opens in your default browser.
   - The backend should be running at **https://localhost:5000/**.

---

### Frontend Configurations

1. **Install Node.js and npm:**
   - If you don't have Node.js and npm installed, download and install them from the [Node.js website](https://nodejs.org/en).

2. **Install Project Dependencies:**
   - Open a terminal in the project directory.
   - Run:
     ```bash
     npm install
     ```

3. **Install Angular CLI:**
   - Install the Angular CLI globally by running:
     ```bash
     npm install -g @angular/cli
     ```

4. **Run the Frontend:**
   - Start the Angular development server by running:
     ```bash
     ng serve
     ```
   - The frontend will be available at **http://localhost:4200/**.

---

### Final Notes
- Ensure to adjust any configuration details according to your specific environment.
- If you encounter any issues, check the console output for errors and verify that all prerequisites are installed and properly configured.
