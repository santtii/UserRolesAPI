namespace UserRolesAPI.Core.Models;

public class RolePermissionsModel
{
    public int RoleId { get; set; }
    public List<int> PermissionIds { get; set; } = new List<int>();
}
