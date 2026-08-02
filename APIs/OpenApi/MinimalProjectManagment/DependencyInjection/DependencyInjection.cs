using System.Text.Json.Serialization;
using Asp.Versioning;
using ControllerProjectManagement.Data;
using ControllerProjectManagement.ExceptionHandler;
using ControllerProjectManagement.OpenApi.Transofrmers;
using ControllerProjectManagement.Permission;
using ControllerProjectManagement.Services;
using ControllerProjectManagement.Validations;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace ControllerProjectManagement.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            // Add your application services here
            services
                .AddCustomProblemDetails()
                .AddCustomApiVersioning()
                .AddCustomOpenApi()
                .AddControllersWithJsonConfiguration()
                .AddDataBase(configuration)
                .AddCustomJwtAuthentication(configuration)
                .AddCustomValidations()
                .AddCustomExceptionHandling()
                .AddCustomServices();
            return services;
        }

        private static IServiceCollection AddCustomProblemDetails(this IServiceCollection services)
        {
            services.AddProblemDetails(op =>
            {
                op.CustomizeProblemDetails = (context) =>
                {
                    context.ProblemDetails.Instance =
                        $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
                    if (!context.ProblemDetails.Extensions.ContainsKey("traceId"))
                    {
                        context.ProblemDetails.Extensions.Add(
                            "traceId",
                            context.HttpContext.TraceIdentifier);
                    }
                };
            });
            return services;
        }
        private static IServiceCollection AddCustomOpenApi(this IServiceCollection services)
        {
            services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer<VersioningTransformers>();
                options.AddDocumentTransformer<BearerSecurityTransformers>();
                options.AddOperationTransformer<BearerSecurityTransformers>();
            });
            return services;
        }
        private static IServiceCollection AddCustomApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            }).AddMvc();
            return services;
        }

        private static IServiceCollection AddControllersWithJsonConfiguration(
            this IServiceCollection services
        )
        {
            services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });
            return services;
        }

        private static IServiceCollection AddDataBase(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(
                    configuration.GetConnectionString("DefaultConnection") ?? "Data Source = app.db"
                )
            );
            return services;
        }

        private static IServiceCollection AddCustomJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration
        )
        {
            // 1. REGISTER AUTHENTICATION SCHEMES
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                // 2. CONFIGURE THE JWT HANDLER
                .AddJwtBearer(options =>
                {
                    var jwtsettings = configuration.GetSection("JwtSettings");

                    options.TokenValidationParameters =
                        new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = jwtsettings["Issuer"],
                            ValidAudience = jwtsettings["Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(
                                System.Text.Encoding.UTF8.GetBytes(
                                    jwtsettings["SecretKey"]
                                        ?? throw new InvalidOperationException(
                                            "SecretKey is not configured in appsettings.json"
                                        )
                                )
                            ),
                        };
                });
            services.AddAuthorization(options =>
            {
                // Project Permissions
                options.AddPolicy(
                    PermissionRoot.Project.Create,
                    policy => policy.RequireClaim("Permission", PermissionRoot.Project.Create)
                );
                options.AddPolicy(
                    PermissionRoot.Project.Read,
                    policy => policy.RequireClaim("Permission", PermissionRoot.Project.Read)
                );
                options.AddPolicy(
                    PermissionRoot.Project.Update,
                    policy => policy.RequireClaim("Permission", PermissionRoot.Project.Update)
                );
                options.AddPolicy(
                    PermissionRoot.Project.Delete,
                    policy => policy.RequireClaim("Permission", PermissionRoot.Project.Delete)
                );
                options.AddPolicy(
                    PermissionRoot.Project.ManageBudget,
                    policy => policy.RequireClaim("Permission", PermissionRoot.Project.ManageBudget)
                );

                // Task Permissions
                options.AddPolicy(
                    PermissionRoot.ProjectTask.Create,
                    policy => policy.RequireClaim("Permission", PermissionRoot.ProjectTask.Create)
                );
                options.AddPolicy(
                    PermissionRoot.ProjectTask.Read,
                    policy => policy.RequireClaim("Permission", PermissionRoot.ProjectTask.Read)
                );
                options.AddPolicy(
                    PermissionRoot.ProjectTask.Update,
                    policy => policy.RequireClaim("Permission", PermissionRoot.ProjectTask.Update)
                );
                options.AddPolicy(
                    PermissionRoot.ProjectTask.Delete,
                    policy => policy.RequireClaim("Permission", PermissionRoot.ProjectTask.Delete)
                );
                options.AddPolicy(
                    PermissionRoot.ProjectTask.AssignUser,
                    policy =>
                        policy.RequireClaim("Permission", PermissionRoot.ProjectTask.AssignUser)
                );
                options.AddPolicy(
                    PermissionRoot.ProjectTask.UpdateStatus,
                    policy =>
                        policy.RequireClaim("Permission", PermissionRoot.ProjectTask.UpdateStatus)
                );
                options.AddPolicy(
                    PermissionRoot.ProjectTask.Comment,
                    policy => policy.RequireClaim("Permission", PermissionRoot.ProjectTask.Comment)
                );
            });

            return services;
        }

        private static IServiceCollection AddCustomValidations(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<AssignUserToTaskRequestValidator>();
            return services;
        }

        private static IServiceCollection AddCustomExceptionHandling(
            this IServiceCollection services
        )
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
            return services;
        }

        private static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            services.AddScoped<IProjectServices, ProjectServices>();
            services.AddScoped<JWTTokenProvider>();
            return services;
        }
    }
}
