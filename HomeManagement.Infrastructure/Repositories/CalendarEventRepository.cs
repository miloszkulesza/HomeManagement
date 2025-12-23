using HomeManagement.Core.Entities;
using HomeManagement.Core.Interfaces.Repositories;
using HomeManagement.Infrastructure.Database;

namespace HomeManagement.Infrastructure.Repositories
{
    public class CalendarEventRepository : Repository<CalendarEvent>, ICalendarEventRepository
    {
        public CalendarEventRepository(HomeManagementContext context) : base(context)
        {
        }
    }
}
