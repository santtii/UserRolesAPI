using UserRolesAPI.Core.Entities;
using UserRolesAPI.Core.Models;
using UserRolesAPI.SharedKernel;

namespace UserRolesAPI.Core.Interfaces;

public interface IUserService
{
    IQueryable<UserEntity> GetAllUsers();
    Task<UserEntity> GetUserAsync(Guid userId);
    Task<UserEntity> GetUserAsync(string email);
    Task<List<PermissionEntity>> GetUserPermissionsAsync(UserEntity user);
    Task<ModelOrError<UserEntity>> RegisterUserAsync(UserModel model);
    Task UpdateUserAsync(UserEntity user);
    Task<List<RoleEntity>> GetUserRolesAsync(UserEntity user);
    Task AddRolesToUserAsync(UserEntity user, List<int> roles);
    Task RemoveRolesFromUserAsync(UserEntity user, List<int> roles);
}
