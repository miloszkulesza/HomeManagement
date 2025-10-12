using HomeManagement.Infrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HomeManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<HomeManagementContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("HomeManagementConnection"));
            });
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
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

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
