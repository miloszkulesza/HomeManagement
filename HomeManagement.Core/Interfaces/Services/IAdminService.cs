using HomeManagement.Core.DTO;
using HomeManagement.Core.ViewModels;

namespace HomeManagement.Core.Interfaces.Services
{
    public interface IAdminService
    {
        Task<List<ApplicationUserVM>> GetUsers();
        Task<ApplicationUserVM?> GetUser(string email);
        Task<List<IdentityRoleVM>> GetRoles();
        Task<ApplicationUserVM?> GetUserById(string id);
        Task<ApplicationUserVM> UpdatePutUserProfile(string id, ApplicationUserUpdateDTO dto);
    }
}
