# PeerEvalApp
An efficient platform for conducting and managing peer and supervisor evaluations within companies, structured around workgroups

## Table of Contents
- [About The Project](#about-the-project)
- [Features](#features)
- [Built With](#built-with)
- [Documentation](#documentation)

## About the Project
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
- **Database**:
  - Microsoft SQL Server
- **API**:
  - .NET  8.0 & C# 12
- **WebApp**:
  - Angular

## Documentation
- **DB Schema**: The database schema is available [here](./DiagramsAndDocumentation/PeerEvalAppDbSchema.svg). For an interactive version, visit [dbdocs.io](https://dbdocs.io/konbarbou/PeerEvalAppDbSchema).
- **Class Diagram**: Visualize the application's architecture [here](./DiagramsAndDocumentation/PeerEvalAppClassDiagram.svg).
- **Role-Based Overview**: Detailed workflows based on user roles can be checked [here](./DiagramsAndDocumentation/NotesWorkFlows.md).
- **Endpoint Overview**: For a detailed API endpoint overview similar to Swagger, go [here](./DiagramsAndDocumentation/EndpointOverview.md).

## Start Up 
- Open SSMS and create a new DataBase name "PeerEvalDB"
- Set a Login as its owner 

