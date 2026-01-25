namespace HomeManagement.Application.ViewModels
{
    public class IdentityRoleVM
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? NormalizedName { get; set; }
        public List<string>? Users { get; set; }
    }
}
