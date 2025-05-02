# RequestLoggingDemo

This project demonstrates how to implement custom middleware in ASP.NET Core to log HTTP requests and responses. Logs are saved both to a text file and a SQL Server database, using Entity Framework Core.

## 🔧 Technologies Used
- ASP.NET Core 8.0
- Entity Framework Core 9.0
- SQL Server
- Middleware Pattern
- Dependency Injection

## 📁 Project Structure

RequestLoggingDemo/
│
├── Middleware/
│   └── RequestLoggingMiddleware.cs
│
├── Models/
│   └── RequestLog.cs
│
├── Data/
│   └── LoggingDbContext.cs
│
├── appsettings.json
└── Program.cs

## ⚙️ Setup Instructions

1. Clone the Repository

``` bash
git clone <repository-url>
cd RequestLoggingDemo
```

2. Configure Your Database

Edit appsettings.json to include your SQL Server connection string:

``` bash
"ConnectionStrings": {
  "DefaultConnection": "Server=your_server;Database=your_db;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

Replace your_server and your_db with your actual server and database name if different.

3. Install EF Core NuGet Packages

``` bash
Install-Package Microsoft.EntityFrameworkCore -Version 9.0.0
Install-Package Microsoft.EntityFrameworkCore.SqlServer -Version 9.0.0
Install-Package Microsoft.EntityFrameworkCore.Tools -Version 9.0.0
```

4. Add and Apply Migrations

``` bash
Add-Migration InitialCreate
Update-Database
```

5. Run the Application

``` bash
dotnet run
```

Send requests via browser, Postman, or curl.

## 📝 Log Output

- Text File: Logs are stored in logs.txt (created in project root).
- Database Table: Logs are stored in the RequestLogs table.

## 🔍 Sample Log Entry

``` bash
Request: {
  "HttpMethod": "GET",
  "Url": "/weatherforecast",
  "Headers": {
    "Accept": ["*/*"]
  },
  "IpAddress": "127.0.0.1"
}
Response: {
  "StatusCode": 200,
  "ResponseTime": "45 ms"
}
```

## 📌 Middleware Features

- Captures:
  - HTTP method
  - URL
  - Headers
  - Client IP address
  - Status code
  - Response time
- Stores logs:
  - To logs.txt
  - To SQL Server via RequestLogs table
- Uses IServiceScopeFactory to access LoggingDbContext safely inside middleware

## 📂 Extensibility Ideas

- Integrate Serilog or NLog for richer structured logging
- Filter logs by HTTP status or URL
- Add correlation IDs for distributed tracing

## 🙋 Troubleshooting

- Ensure the correct .NET 8 SDK is installed.
- Double-check connection string formatting in appsettings.json.
- Verify EF Core packages are installed.
- Run Visual Studio as Administrator if needed for database access.

## 📄 License

Free to use and modify. Originally based on FreeCodeSpot (https://freecodespot.in).
