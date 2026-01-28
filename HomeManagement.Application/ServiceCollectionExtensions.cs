using Microsoft.Extensions.DependencyInjection;
using HomeManagement.Core.Interfaces.Services;
using HomeManagement.Application.Services;

namespace HomeManagement.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddMaps(typeof(ApplicationAssemblyMarker).Assembly);
            });

            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<ICalendarEventService, CalendarEventService>();
            services.AddScoped<IWorkItemService, WorkItemService>();

            return services;
        }
    }
}