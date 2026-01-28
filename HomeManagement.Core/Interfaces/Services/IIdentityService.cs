using HomeManagement.Core.Entities;

namespace HomeManagement.Core.Interfaces.Services
{
    public interface IIdentityService
    {
        Task<User?> GetUserByEmail(string email);
        Task<User?> GetUserById(string id);
        Task<List<User>> GetUsers();
        Task<IList<string>> GetUserRolesAsync(string userId);
        Task<List<Role>> GetRoles();
        Task UpdateUserAsync(string id, User user);
    }
}