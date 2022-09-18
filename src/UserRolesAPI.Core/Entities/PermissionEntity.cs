using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using UserRolesAPI.SharedKernel;

namespace UserRolesAPI.Core.Entities;

public class PermissionEntity : EntityBase
{
    [Key]
    public int Id { get; set; }
    public string Permission { get; set; } = string.Empty;

    public virtual ICollection<RoleEntity>? Roles { get; set; } = new Collection<RoleEntity>();
}
