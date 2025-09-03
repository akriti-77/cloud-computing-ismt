# ISMT College Website

A modern, responsive college website built with ASP.NET Core 6 MVC and PostgreSQL, designed for cloud migration and deployment.

## Features

- **Modern UI/UX**: Clean, responsive design with Bootstrap 5 and Font Awesome icons
- **Contact Form**: Functional contact form with PostgreSQL database storage
- **Navigation**: Complete navigation structure with Home, About, Courses, and Contact pages
- **Database Integration**: Entity Framework Core with PostgreSQL
- **Cloud Ready**: Configured for both local development and Azure deployment

## Technology Stack

- **Backend**: ASP.NET Core 6 MVC
- **Database**: PostgreSQL with Entity Framework Core
- **Frontend**: Bootstrap 5, Font Awesome, HTML5, CSS3
- **ORM**: Entity Framework Core 6.0.29
- **Database Provider**: Npgsql.EntityFrameworkCore.PostgreSQL 6.0.29

## Prerequisites

- .NET 6.0 SDK
- PostgreSQL 12 or later
- Visual Studio 2022 or VS Code (optional)

## Local Development Setup

### 1. Clone and Navigate to Project
```bash
cd ISMTCollege
```

### 2. Install Dependencies
```bash
dotnet restore
```

### 3. Configure Database Connection
Update the connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=ismtcollege;Username=your_username;Password=your_password;Port=5432"
  }
}
```

### 4. Create PostgreSQL Database
```sql
CREATE DATABASE ismtcollege;
```

### 5. Run Entity Framework Migrations
```bash
# Install EF Core CLI tools globally (if not already installed)
dotnet tool install --global dotnet-ef

# Create and apply migrations
dotnet ef database update
```

### 6. Run the Application
```bash
dotnet run
```

The application will be available at `https://localhost:7000` or `http://localhost:5000`

## Project Structure

```
ISMTCollege/
├── Controllers/          # MVC Controllers
│   ├── HomeController.cs
│   ├── AboutController.cs
│   ├── CoursesController.cs
│   └── ContactController.cs
├── Models/               # Data Models
│   └── Contact.cs
├── Views/                # Razor Views
│   ├── Home/
│   ├── About/
│   ├── Courses/
│   └── Contact/
├── Data/                 # Database Context
│   └── ApplicationDbContext.cs
├── Migrations/           # EF Core Migrations
├── wwwroot/             # Static Files
└── Program.cs           # Application Entry Point
```

## Database Schema

### Contacts Table
- `Id` (int, Primary Key)
- `Name` (string, Required, Max 100 chars)
- `Email` (string, Required, Max 150 chars, Email format)
- `Message` (string, Required, Max 1000 chars)
- `CreatedAt` (DateTime, Auto-generated)

## Azure Deployment

### 1. Azure App Service Setup
- Create a new App Service with .NET 6 runtime
- Configure environment variables for connection strings

### 2. Azure Database for PostgreSQL
- Create Azure Database for PostgreSQL Flexible Server
- Update connection string in Azure App Service Configuration

### 3. Connection String for Azure
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=your-server.postgres.database.azure.com;Database=ismtcollege;Username=your_username;Password=your_password;Port=5432;SSL Mode=Require;Trust Server Certificate=true;"
  }
}
```

### 4. Deploy to Azure
```bash
# Publish the application
dotnet publish -c Release -o ./publish

# Deploy to Azure App Service using Azure CLI or Azure DevOps
```

## Environment Variables

For production deployment, set these environment variables in Azure App Service:

- `ConnectionStrings__DefaultConnection`: Your PostgreSQL connection string
- `ASPNETCORE_ENVIRONMENT`: Production

## Local Testing

1. **Home Page**: Navigate to `/` to see the college homepage
2. **About Page**: Visit `/About` for college information
3. **Courses Page**: Check `/Courses` for academic programs
4. **Contact Form**: Test the contact form at `/Contact`

## Troubleshooting

### Common Issues

1. **Database Connection Error**
   - Verify PostgreSQL is running
   - Check connection string in `appsettings.json`
   - Ensure database exists

2. **Migration Errors**
   - Delete Migrations folder and recreate: `dotnet ef migrations add InitialCreate`
   - Ensure database is accessible

3. **Build Errors**
   - Run `dotnet restore` to restore packages
   - Verify .NET 6 SDK is installed

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## License

This project is created for educational purposes as part of a cloud computing assignment.

## Support

For support or questions, please contact the development team or refer to the Azure documentation for deployment assistance.
