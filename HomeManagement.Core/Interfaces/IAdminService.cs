using HomeManagement.Core.ViewModels;

namespace HomeManagement.Core.Interfaces
{
    public interface IAdminService
    {
        Task<List<ApplicationUserVM>> GetUsers();
        Task<ApplicationUserVM?> GetUser(string email);
        Task<List<IdentityRoleVM>> GetRoles();
    }
}
