using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UserRolesAPI.SharedKernel;

namespace UserRolesAPI.Core.Entities;

public class RoleEntity : EntityBase
{
    [Key]
    public int Id { get; set; }
    [ForeignKey("ParentRole")]
    public int? ParentId { get; set; }
    public string Role { get; set; } = string.Empty;
    public bool Default { get; set; }

    public virtual RoleEntity? ParentRole { get; set; }

    public virtual ICollection<PermissionEntity> Permissions { get; set; } = new Collection<PermissionEntity>();
    public virtual ICollection<UserEntity> Users { get; set; } = new Collection<UserEntity>();
    public virtual ICollection<RoleEntity> Childrens { get; set; } = new Collection<RoleEntity>();
}
