ClothWebApp (API base).
Director & Coder: Jinkey
Technologies & libraries: C#/.NET 6.0 (Web API), EF Core, Dapper, MediatR, SignalR, ASP.NET Identity, SQL Server,...

This is a simple web api that implements Vertical Slice Architecture for developing.
The architecture separates project into 4 smaller projects, consist of:
* .Data (Represent for Data layer, which contains data (entities) and some actions such as database connection, entity configurations and data seeding.
* .Handler (Represent for Domain layer, which contains most of business logic of application such as CRUD entities, real-time notification,...)
* .Share (A project which contains a lot of classes that will use entire solution)
* .Web (Main project which represent for Representation layer, the entry point of application, this will configure Services, Pipeline and something else for website running.
