namespace UserRolesAPI.Core.Models;

public class UserRolesModel : CredentialsModel
{
    public Guid UserId { get; set; }
    public List<int> RoleIds { get; set; } = new List<int>();
}
