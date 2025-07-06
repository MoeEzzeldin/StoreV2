
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Store.Models;

namespace Store.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusController : ControllerBase
    {
        private readonly ILogger<StatusController> _logger;
        private readonly IHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private static readonly DateTime _serverStartTime = DateTime.UtcNow;

        public StatusController(
            ILogger<StatusController> logger,
            IHostEnvironment environment,
            IConfiguration configuration,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _environment = environment;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        [HttpGet]
        public ActionResult<ServerStatus> Get()
        {
            var status = new ServerStatus
            {
                IsOnline = true,
                Message = "Server is online",
                StartTime = _serverStartTime,
                
                // System information
                HostName = Environment.MachineName,
                OSVersion = RuntimeInformation.OSDescription,
                RuntimeVersion = RuntimeInformation.FrameworkDescription,
                ProcessorCount = Environment.ProcessorCount,
                
                // Application information
                ApplicationName = Assembly.GetEntryAssembly()?.GetName().Name,
                Version = Assembly.GetEntryAssembly()?.GetName().Version?.ToString() ?? "1.0.0",
                Environment = _environment.EnvironmentName,
                
                // Database information
                DatabaseConnected = CheckDatabaseConnection(),
                
                // Endpoints information
                Endpoints = new List<string>
                {
                    "/api/status",
                    "/api/products"
                }
            };
            
            _logger.LogInformation("Server status checked at {Time}. Status: {Status}, Uptime: {Uptime}", 
                DateTime.UtcNow, status.Message, status.Uptime);
                
            return status;
        }
        
        private bool CheckDatabaseConnection()
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");

                if (string.IsNullOrEmpty(connectionString))
                {
                    _logger.LogWarning("Connection string is null or empty");
                    return false;
                }

                using var connection = new SqlConnection(connectionString);
                _logger.LogInformation("Attempting to open database connection...");
                connection.Open();
                
                // Run a simple query to verify database is accessible
                using var command = connection.CreateCommand();
                command.CommandText = "SELECT COUNT(*) FROM product";
                var count = command.ExecuteScalar();
                _logger.LogInformation("Database connection successful. Product count: {Count}", count);
                
                return true;
            }
            catch (SqlException ex)
            {
                _logger.LogWarning("SQL Exception: Code {ErrorCode}, Message: {ErrorMessage}", 
                    ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Database connection check failed with error: {ErrorMessage}", 
                    ex.Message);
                return false;
            }
        }
    }
}
