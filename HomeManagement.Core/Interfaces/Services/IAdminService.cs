using HomeManagement.Core.Entities;

namespace HomeManagement.Core.Interfaces.Services
{
    public interface IAdminService
    {
        Task<User?> GetUser(string email);
        Task<User?> GetUserById(string id);
         Task<List<User>> GetUsers();
        Task<List<Role>> GetRoles();
        Task<User> UpdatePutUserProfile(string id, User user);
    }
}
