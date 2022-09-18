namespace UserRolesAPI.Core.Models;

public class RoleModel
{
    public int? ParentId { get; set; }
    public string Role { get; set; } = string.Empty;
    public bool Default { get; set; } = false;
}
