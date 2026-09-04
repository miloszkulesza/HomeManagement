using HomeManagement.Application;
using HomeManagement.Core.Consts;
using HomeManagement.Infrastructure;
using HomeManagement.Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi;
using System.Reflection;

namespace HomeManagement
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var allowedHosts = builder.Configuration.GetSection("CORS:AllowedHosts").Get<string[]>();
            builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(allowedHosts ?? Array.Empty<string>())
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            }));

            builder.Services.AddSingleton(TimeProvider.System);
            builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<HomeManagementContext>();

            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();

            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);

            builder.Services.AddControllers();
            builder.Services.AddProblemDetails();
            builder.Services.AddExceptionHandler<ApiExceptionHandler>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Home Management API", Version = "v1" });
                c.EnableAnnotations();
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }

                var bearerScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Wprowadź token JWT w formacie: Bearer {token}",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                };
                c.AddSecurityDefinition("Bearer", bearerScheme);
                c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                });
            });

            var app = builder.Build();

            app.UseExceptionHandler();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(c => c.RouteTemplate = "openapi/{documentName}.json");

                app.UseReDoc(opt =>
                {
                    opt.RoutePrefix = "docs";
                    opt.DocumentTitle = "Home Management API";
                    opt.SpecUrl("/openapi/v1.json");
                });
            }

            app.UseCors();

            using var scope = app.Services.CreateScope();
            await InitializeDatabase.Seed(scope.ServiceProvider);

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapGroup("/api")
                .MapControllers();
            var identityEndpoints = app.MapGroup("/api/auth")
                .MapIdentityApi<ApplicationUser>();

            identityEndpoints.Add(endpointBuilder =>
            {
                if (endpointBuilder is not RouteEndpointBuilder routeEndpointBuilder)
                    return;

                var lastSegment = routeEndpointBuilder.RoutePattern.RawText?
                    .TrimEnd('/')
                    .Split('/')
                    .LastOrDefault();

                if (string.Equals(lastSegment, "register", StringComparison.OrdinalIgnoreCase))
                    endpointBuilder.Metadata.Add(new AuthorizeAttribute { Roles = Roles.Admin });
            });

            app.Run();
        }
    }
}
