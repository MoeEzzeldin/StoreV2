# Web Store API

A .NET 8 API for a web store application.

## Setup Instructions

### Prerequisites
- .NET 8 SDK
- SQL Server
- Visual Studio 2022 or another compatible IDE

### Database Setup
1. Open SQL Server Management Studio or another SQL client
2. Run the database setup script located at `/database/Store.sql`
3. This will create the necessary database and tables with sample data

### Configuration
1. Copy `appsettings.template.json` to `appsettings.json` (if not already present)
2. Update the connection string and other sensitive information in `appsettings.json`
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=your-server;Database=store;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```
3. Update the JWT secret key
   ```json
   "JwtSecret": "your-secure-secret-key-here"
   ```

### Running the Application
1. Open the solution in Visual Studio
2. Build the solution
3. Run the application
4. The API will start and open the status page at `/api/status`

## Available Endpoints
- `/api/status` - Get server status information
- `/api/products` - Access product data

## Security Notes
- `appsettings.json` contains sensitive information and is excluded from version control via `.gitignore`
- Use the template file as a reference for required configuration
- In production, consider using environment variables or a secure vault for sensitive information