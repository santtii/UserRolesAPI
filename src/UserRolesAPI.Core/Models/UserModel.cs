using UserRolesAPI.Core.Entities;

namespace UserRolesAPI.Core.Models;

public class UserModel : CredentialsModel
{
    public string Name { get; set; } = string.Empty;
    public string Departament { get; set; } = string.Empty;

    public static explicit operator UserEntity(UserModel model)
    {
        return new UserEntity
        {
            Name = model.Name,
            Email = model.Email,
            Departament = model.Departament,
            Password = model.Password
        };
    }
}
