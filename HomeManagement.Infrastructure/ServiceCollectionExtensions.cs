using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using HomeManagement.Core.Interfaces.Repositories;
using HomeManagement.Core.Interfaces.Services;
using HomeManagement.Infrastructure.Repositories;
using HomeManagement.Infrastructure.Services;
using HomeManagement.Infrastructure.Database;

namespace HomeManagement.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<HomeManagementContext>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("HomeManagementConnection")));

            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(typeof(InfrastructureAssemblyMarker).Assembly);
            });

            services.AddScoped<ICalendarEventRepository, CalendarEventRepository>();
            services.AddScoped<IWorkItemRepository, WorkItemRepository>();

            services.AddScoped<IIdentityService, IdentityService>();

            return services;
        }
    }
}