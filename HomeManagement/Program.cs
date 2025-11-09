using HomeManagement.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HomeManagement
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var allowedHosts = builder.Configuration.GetSection("CORS:AllowedHosts").Get<string[]>();
            builder.Services.AddCors(options => options.AddDefaultPolicy(builder =>
            {
                builder.WithOrigins(allowedHosts!)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin();

            }));
            builder.Services.AddDbContext<HomeManagementContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("HomeManagementConnection"));
            });
            builder.Services.AddSingleton(TimeProvider.System);
            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();
            builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<HomeManagementContext>();
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            
            var app = builder.Build();
            
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
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
            app.MapGroup("/api/auth")
                .MapIdentityApi<ApplicationUser>();

            app.Run();
        }
    }
}
