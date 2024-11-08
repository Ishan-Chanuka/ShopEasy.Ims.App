# ShopEasy Inventory Management System - Backend

This is the backend for the Inventory Management System developed for the ShopEasy e-commerce company. It provides RESTful API endpoints to manage inventory and supports various operations for Admin and Employee roles.

## Getting Started

### Prerequisites

Ensure you have the following installed:
- [.NET SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Installation and Setup

1. **Clone this repository:**
   ```
   git clone git@github.com:Ishan-Chanuka/ShopEasy.Ims.App.git
   ```
   
2. **Restore NuGet packages:**
   Run the following command in the root directory
  ```
   dotnet restore
  ```

3. **Configure the Database Connection:**
   Open `appsettings.json` in the project directory, and update the connection string to match your SQL Server configuration.
   
4. **Initialize the Database:**
   Open a terminal in the `src/Core/Infrastructure` directory and run
  ```
   update-database
  ```

5. **Run the Application:**
   Open the project with the `.sln` file in the root directory or navigate to `src/Core/ShopEasy.Ims.Api` in a terminal and execute the following commands:
```
dotnet clean
dotnet build
dotnet run
```

Once the application is running, open your web browser and go to:
```
http://localhost:5265/swagger/index.html
```

6. **Alternative Run Option via Solution Explorer:**
   If using an IDE (e.g., Visual Studio), right-click on the `.sln` file in the Solution Explorer. Then, clean, build, and run the project.

### Testing the API

To access the API endpoints, you’ll need a JWT token. Use the following credentials to obtain a token for different roles:
**Admin Role**
- Email: `admin@abc.net`
- Password: `Admin@123`

**Employee Role**
- Email: `employee@abc.net`
- Password: `Employee@123`

Authenticate via the `/login `endpoint to retrieve a JWT token, which you can then use in the `Authorization` header for other API requests.

### Additional Notes

- **Swagger UI:** The Swagger UI at `/swagger/index.html` provides detailed documentation and allows you to interact with the API directly.
- **Configuration:** The application’s configurations can be customized in the `appsettings.json` file.

## Technologies Used

- .NET 8
- Entity Framework Core
- SQL Server
- Swagger/OpenAPI
- JWT Authentication
- QuestPDF
