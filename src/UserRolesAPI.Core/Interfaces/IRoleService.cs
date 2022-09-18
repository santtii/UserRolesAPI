using UserRolesAPI.Core.Entities;
using UserRolesAPI.Core.Models;
using UserRolesAPI.SharedKernel;

namespace UserRolesAPI.Core.Interfaces;

public interface IRoleService
{
    Task<RoleEntity> GetRoleAsync(int roleId);
    Task<List<RoleEntity>> GetRolesAsync();
    Task<ModelOrError<RoleEntity>> AddRoleAsync(RoleModel model);
    Task<ModelOrError<bool>> UpdateRoleAsync(RoleEntity role);
    Task<List<PermissionEntity>> GetRolePermissionsAsync(RoleEntity role);
    Task AddPermissionsToRoleAsync(RoleEntity role, List<int> permissions);
    Task RemovePermissionsFromRoleAsync(RoleEntity role, List<int> roles);
}
