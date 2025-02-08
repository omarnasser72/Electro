using Electro.APIs.Profiles;
using Electro.Core.Entities;
using Electro.Core.Interfaces;
using Electro.Core.Interfaces.Repositories;
using Electro.Repository.Contexts;
using Electro.Repository.Repositories;
using Electro.Repository.UnitOfWork;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Electro.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
                    /*
                     * This line is related to handling circular references or object references when serializing data.
                     * Background on Circular References
                     *      In object-oriented programming, circular references occur when objects reference each other in 
                     *      a loop. For example, if object A contains a reference to object B, and object B contains 
                     *      a reference back to object A, you have a circular reference. During serialization 
                     *      (converting objects to JSON), circular references can cause infinite loops unless 
                     *      explicitly handled.
                     * 
                     * 
                     * ReferenceHandler Explanation
                     *      The ReferenceHandler property in System.Text.Json is used to manage how object references 
                     *      (including circular references) are serialized. When ReferenceHandler is set to 
                     *      a non-null value, it manages reference cycles and object identity using either:
                     *
                     *      ReferenceHandler.Preserve: This setting tracks references during serialization, 
                     *                                 emitting $id and $ref metadata (such as what you saw with $id: "1").
                     *                                 This prevents infinite loops by using a form of "reference tracking."
                     *
                     *      ReferenceHandler.IgnoreCycles: Ignores circular references and does not serialize them, 
                     *                                     preventing errors or infinite loops, but may result in 
                     *                                     incomplete data.
                     * 
                     * Why Set to null?
                     *      By setting ReferenceHandler = null, you're telling the serializer not to use any reference 
                     *      handling mechanisms. This prevents the serialization of $id fields or other metadata, 
                     *      as you're opting to handle the data yourself or ensuring that your object structure 
                     *      doesn't contain circular references. This setting assumes you don't need reference tracking.
                     */
                    //options.JsonSerializerOptions.ReferenceHandler = null;

                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;

                    /*
                     * What does DefaultIgnoreCondition do?
                     * This property controls the behavior of the serializer regarding null values. 
                     * Specifically, it tells the serializer whether or not to include properties with 
                     * null values in the JSON output.
                     *
                     * JsonIgnoreCondition.WhenWritingNull Explanation
                     *      When this condition is set to WhenWritingNull, it instructs the serializer 
                     *      to omit properties that have null values from the JSON output. 
                     *      This means if an object has a property with a null value, 
                     *      that property will be excluded from the final JSON result.
                     */
                    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;

                });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAutoMapper(typeof(BaseProfile));

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped(/*typeof(IUnitOfWork<>), */typeof(UnitOfWork<>));

            builder.Services.AddDbContext<ElectroDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), X => X.UseDateOnlyTimeOnly());
            });

            /*
             * This method sets up authentication in the ASP.NET Core dependency injection (DI) container.
             * CookieAuthenticationDefaults.AuthenticationScheme is the default scheme for handling cookies. 
             * A scheme is just a way to define how authentication will be handled. You can have multiple 
             * schemes, but here, we are using the default cookie scheme.
             * This ensures that the application will use cookies to manage the authentication lifecycle.
             */
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                            .AddCookie(options =>
                            {
                                /*
             * This method is used to configure how cookie-based authentication will work in detail.
             * options.LoginPath = "Account/Login":
             *      Defines the URL to which the user is redirected if they try to access a protected resource
             *      without being authenticated.
             *      In this case, if an unauthenticated user tries to access a page that requires login, 
             *      they will be redirected to /Account/Login.
             *      You can replace Account/Login with any path where your login action resides.
             *
             * options.AccessDeniedPath = "/Home/Error":
             *      Defines the URL where users will be redirected if they attempt to access a resource
             *      they are not authorized to use (i.e., they are authenticated, but they lack the 
             *      necessary permissions).
             *      If the user is authenticated but tries to access a page that they are not allowed to,
             *      they will be sent to the /Home/Error page, which typically displays an error message 
             *      such as "Access Denied."
             *
             */
                                options.AccessDeniedPath = "Error";
                                options.LoginPath = "/login";
                                options.LogoutPath = "/logout";
                                options.Cookie.HttpOnly = true;
                            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
                options.AddPolicy("RequireCustomerRole", policy => policy.RequireRole("Customer"));
            });

            /*
             * What is ASP.NET Core Identity?
             * ASP.NET Core Identity is a membership system that adds login functionality to your application. 
             *
             * It provides functionality for:
             *      Creating and managing user accounts.
             *      Handling user roles and claims.
             *      Password management (including password hashing).
             *      Two-factor authentication (2FA).
             *      Token generation for things like password resets or email confirmation.
             */
            /*
             * This method adds ASP.NET Core Identity to the DI container, specifying that:
             *      ApplicationUser: Represents the user class (which usually inherits from IdentityUser).
             *      IdentityRole: Represents the role class (which can be used to define roles like 
             *      "Admin", "User", etc.).
             * ASP.NET Core Identity provides default implementations for these classes, 
             * but you can extend them with custom properties if needed. 
             * For example, you might extend ApplicationUser to include properties like ProfilePictureUrl 
             * or DateOfBirth.
             */
            builder.Services.AddIdentity<User, IdentityRole>()
            /*
             * Configures Identity to use Entity Framework Core for storing user and role data 
             * in a relational database.
             * The MvcDbContext is the database context that represents the database where 
             * Identity information will be stored. It must inherit from IdentityDbContext<ApplicationUser>
             * (or IdentityDbContext if you don't extend ApplicationUser).
             * This allows the Identity system to use the database for user management 
             * (e.g., storing user credentials, roles, and claims).
             */
            .AddEntityFrameworkStores<ElectroDbContext>()
            /*
             * This method adds default token providers used in ASP.NET Core Identity for generating secure 
             * tokens for tasks like:
             *      Password resets: When a user forgets their password, Identity generates a secure token 
             *      that can be used to reset the password.
             *      
             *      Email confirmation: When a user registers, Identity can generate a token to confirm 
             *      the user's email address.
             *      
             *      Two-factor authentication (2FA): Tokens can also be used for verifying users in 
             *      two-factor authentication processes.
             * 
             * These token providers are crucial for security, ensuring that sensitive operations (like password resets) are protected.
             */
            .AddDefaultTokenProviders();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            #region Update Database
            #region Summary
            /*
             * Why this section exists: 
             *   The primary goal of this block is to ensure that the database schema matches the current model definitions 
             *   when the application starts by automatically applying any pending migrations.
             * How it works:
             *   A new scope is created to handle scoped services (CreateScope()).
             *   The ServiceProvider is used to resolve services like DbContext.
             *   TalabatDbContext is retrieved from the DI container, which allows interaction with the database.
             *   MigrateAsync() is called to apply any pending migrations to the database schema, ensuring the database is always up-to-date with the application models.
             */
            #endregion
            #region My Summary
            //we're creating scope explicitly to be not depending on http request and
            //we get services from that created instance and getting DbContext
            //to apply migrations asynchronously whenever changes are made to models
            #endregion

            #region Scope
            /*
             * Context: 
             *      In ASP.NET Core, services are often registered with different lifetimes: Singleton, Scoped, and Transient. 
             *      Services registered as Scoped are created once per request and are disposed of when the request ends. 
             *      However, at the startup of the application (where no HTTP request is present), 
             *      you cannot directly resolve a scoped service like DbContext. Instead, you need to create a scope manually.
             *
             * Explanation:
             *      The CreateScope() method creates a new lifetime scope within which scoped services can be resolved. 
             *      Think of it as starting a temporary container where scoped services are available.
             *      The using statement ensures that the scope is disposed of once it's no longer needed. 
             *      This is important for cleaning up resources like database connections, ensuring there are no memory leaks.
             * 
             * Purpose: 
             *      This is necessary because DbContext, which is a scoped service, needs to be available 
             *      for a task outside of a typical request pipeline, such as applying migrations during app startup.
             *
             * Why it's needed: 
             *      Scoped services, like DbContext, are designed to have a limited lifetime. 
             *      --Creating a scope explicitly-- allows the application to manage that lifetime correctly outside of an HTTP request.
             *      
             * Summary:
             *      This line creates a new dependency injection scope and automatically cleans it up after use (due to the using statement).
             */
            #endregion
            using var Scope = app.Services.CreateScope();

            #region Services
            /*
             * Context: 
             *      The ServiceProvider is the mechanism that provides access to all the services registered 
             *      in the Dependency Injection (DI) container of the application. 
             *      The DI container manages services like DbContext, configurations, controllers, etc.
             *
             * Explanation:
             *      The ServiceProvider is used to resolve or retrieve services from the DI container
             *      within the scope that was just created.
             *      It acts as a "service locator," allowing you to get instances of services that have been registered with DI.
             *
             * Purpose: 
             *      This step allows access to all the services registered in the DI container for the current scope.
             *      You need it to resolve DbContext, which will be used to apply the migrations.
             * 
             * Summary:
             *      This line retrieves the ServiceProvider from the created scope, giving access to all 
             *      the services registered in the application.
             *      
             */
            #endregion
            var Services = Scope.ServiceProvider;

            #region LoggerFactory
            /*
             * Context:
             *      Logging in ASP.NET Core is handled by a flexible logging infrastructure that allows developers 
             *      to record runtime information.
             *      The ILoggerFactory is a core part of this logging infrastructure. 
             *      It is responsible for creating ILogger instances, which can log messages 
             *      (e.g., information, warnings, errors) to various providers such as the console, files, 
             *      or external logging services.
             * Explanation:
             *      Services.GetRequiredService<ILoggerFactory>():
             *      This method retrieves the ILoggerFactory service from the Dependency Injection (DI) container.
             *      The ILoggerFactory is typically registered by default in ASP.NET Core when you set up logging 
             *      during application configuration.
             *      If the ILoggerFactory service is not registered, it will throw an exception, 
             *      ensuring that the service is present and configured.
             * Purpose:
             *      The purpose of this line is to retrieve the logging factory so you can create a logger (ILogger) 
             *      specific to the class or component where the logging will be performed (in this case, the Program class).
             * Summary:
             *      This line retrieves the ILoggerFactory, which is needed to create an ILogger instance 
             *      to log messages (errors, warnings, etc.) during the application's execution.
             */
            #endregion
            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();


            try
            {
                #region DbContext
                /*
                 * Context: 
                 *      In ASP.NET Core, DbContext is a service that is typically registered with a scoped lifetime. 
                 *      This means a new instance is created per request and is disposed of at the end of the request. 
                 *      Since you're outside of a request (in the application startup code), 
                 *      you have to resolve the DbContext explicitly from the service provider.
                 *
                 * Explanation:
                 *      GetRequiredService<T> is a method of the ServiceProvider. 
                 *      It retrieves the required service (in this case, the TalabatDbContext) from the DI container. 
                 *      If the service is not registered, it throws an exception.
                 *      In this context, TalabatDbContext represents the database context, 
                 *      which is responsible for interacting with the underlying database.
                 *
                 * Purpose: 
                 *      This step retrieves an instance of the TalabatDbContext from the DI container so that database operations 
                 *      (like migrations) can be performed. Without retrieving DbContext, 
                 *      you cannot access the database or apply migrations.
                 *      
                 * Summary:
                 *      This line resolves an instance of TalabatDbContext (your Entity Framework Core database context) 
                 *      from the DI container within the current scope. 
                 *      If the TalabatDbContext is not properly registered in the DI container, an exception is thrown.
                 *   
                 */
                #endregion
                var DbContext = Services.GetRequiredService<ElectroDbContext>();

                #region Migrate
                /*
                 * Context: 
                 *      Migrations in EF Core are a way to update the database schema based on changes made 
                 *      to the model classes in the application. 
                 *      Instead of manually running database migrations from the command line, 
                 *      you can automate the process by calling MigrateAsync() at startup.
                 *
                 * Explanation:
                 *      MigrateAsync() is an asynchronous method that checks if there are any pending migrations 
                 *      that haven't been applied to the database. If there are, it applies them.
                 *      The method is async, which means it runs in the background without blocking the main thread. 
                 *      This is important in web applications where blocking the main thread can slow down performance.
                 *      The method ensures the database schema is up to date with the current state of your C# models. 
                 *      For example, if you added or removed a property in a model, EF Core will create the corresponding table 
                 *      or column changes and apply them to the database.
                 *
                 * Why it's important:
                 *      Running migrations during application startup ensures that your database is always in sync with the code,
                 *      without requiring manual migration commands.
                 *      This is particularly useful in environments where you might not have access to the database directly 
                 *      (like in cloud deployments), and you want to ensure the database schema is updated when the app starts.
                 */
                #endregion
                await DbContext.Database.MigrateAsync();

            }
            catch (Exception ex)
            {
                #region Logger
                /*
                 * Context:
                 *      The ILogger interface is used for writing log messages. 
                 *      Each log entry records useful information such as a message, severity (e.g., error, warning), 
                 *      and possibly an exception.
                 *      You usually create an ILogger specific to the current class so that when you log messages, 
                 *      they are associated with the correct class (making logs easier to read and trace).
                 * Explanation:
                 *      LoggerFactory.CreateLogger<Program>():
                 *      This method creates an ILogger instance associated with the Program class.
                 *      The generic <Program> type parameter indicates that this logger will be used for logging 
                 *      within the Program class.
                 *      When the logger records messages, it will also include metadata indicating the source of the log 
                 *      (e.g., "Program").
                 * Purpose:
                 *      This line creates a logger specific to the Program class, so any log messages will include information 
                 *      that identifies the Program class as the source of the log entry.
                 * Summary:
                 *      This line creates a logger for the Program class, which will be used to log error messages 
                 *      and other runtime information during execution.
                 */
                #endregion
                var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, "Error occured while appling migrations.");
            }
            #endregion


            // Seed roles when the application starts
            using (var scope = app.Services.CreateScope())
            {
                var RoleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await CreateRoles(RoleManager);
            }

            static async Task CreateRoles(RoleManager<IdentityRole> RoleManager)
            {
                string[] RoleNames = { "Admin", "Customer" };

                foreach (var roleName in RoleNames)
                {
                    var roleExists = await RoleManager.RoleExistsAsync(roleName);
                    if (!roleExists)
                        await RoleManager.CreateAsync(new IdentityRole(roleName));

                }
            }


            app.Run();
        }
    }
}
