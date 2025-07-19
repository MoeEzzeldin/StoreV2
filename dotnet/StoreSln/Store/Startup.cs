using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using Store.Data;
using Store.Models;
using Store.Reposatories.I_Repos;
using Store.Reposatories.Repos;
using Store.Security;
using Store.Utils;
using System.Reflection;
using System.Text;
using Store.Services.I_AppService;
using Store.Services.AppService;


namespace Store
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            // Configure Serilog to log to file and console
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .WriteTo.Console()
                .WriteTo.File("logs/store-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            try
            {
                Log.Information("Starting application");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application startup failed");
                throw; // Re-throw to ensure the application does not start if logging fails
            }
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add controllers and Razor Pages
            services.AddControllers();

            // Configure CORS
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });
            
            // Get connection string from configuration
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                Log.Warning("Connection string is null or empty. The application may not be able to connect to the database.");
            }
            else
            {
                Log.Information("Using connection string: {ConnectionString}", 
                    connectionString.Replace("Password=", "Password=***"));
            }
                
            // Configure DbContext with SQL Server
            services.AddDbContext<ApplicationDbContext>(options => 
                options.UseSqlServer(connectionString ?? "Server=localhost;Database=store;Trusted_Connection=True;TrustServerCertificate=True;"));
            
            // Register DapperContext
            services.AddScoped<IDapperContext, DapperContext>();

            // add AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // Register repository services
            services.AddScoped<I_ProductRepo, ProductRepo>();
            services.AddScoped<I_UserRepo, UserRepo>();
            services.AddScoped<I_ReviewRepo, ReviewRepo>();

            // Register application services
            services.AddScoped<I_ProductService, ProductService>();

            // Configure JWT Authentication
            var jwtSecret = Configuration["JwtSecret"];
            if (string.IsNullOrEmpty(jwtSecret))
            {
                Log.Warning("JWT Secret is null or empty! Using fallback key for development only.");
                jwtSecret = "fallback-key-for-dev-only-do-not-use-in-production";
            }
            
            var key = Encoding.ASCII.GetBytes(jwtSecret);
            
            // Register security services
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<ITokenGenerator>(sp => new JwtGenerator(jwtSecret));
            
            // Configure authentication
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            
            // Configure Serilog for dependency injection
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog(dispose: true);
            });
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            
            app.UseRouting();
            
            app.UseCors();
            

            // Add authentication middleware
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            
            // Log server start
            Log.Information("Server started successfully at {Time}. Environment: {Environment}", 
                DateTime.UtcNow, env.EnvironmentName);
            Log.Information("Server is online and ready to accept requests");
        }
    }
}
