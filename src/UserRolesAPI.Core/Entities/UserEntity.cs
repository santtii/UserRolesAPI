using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using UserRolesAPI.Core.Models;
using UserRolesAPI.SharedKernel;

namespace UserRolesAPI.Core.Entities;

public class UserEntity : EntityBase
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Departament { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public virtual ICollection<RoleEntity> Roles { get; set; } = new Collection<RoleEntity>();

    public static explicit operator UserModel(UserEntity entity)
    {
        return new UserModel
        {
            Name = entity.Name,
            Email = entity.Email,
            Departament = entity.Departament
        };
    }
}
