using UserRolesAPI.Core.Entities;
using UserRolesAPI.Core.Interfaces;
using UserRolesAPI.Core.Interfaces.Data;
using UserRolesAPI.Core.Models;
using UserRolesAPI.SharedKernel;

namespace UserRolesAPI.Core.Services;

public class RoleService : IRoleService
{
    private readonly IRepository _repository;

    public RoleService(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<RoleEntity> GetRoleAsync(int roleId)
    {
        return await _repository.FirstAsync<RoleEntity>(x => x.Id == roleId);
    }

    public async Task<List<RoleEntity>> GetRolesAsync()
    {
        return await _repository.GetAllAsync<RoleEntity>();
    }

    public async Task<ModelOrError<RoleEntity>> AddRoleAsync(RoleModel model)
    {
        var result = new ModelOrError<RoleEntity>();

        var role = await _repository.FirstAsync<RoleEntity>(x => x.Role == model.Role);
        if (role == null)
        {
            result.Model = await _repository.AddAsync(new RoleEntity { ParentId = model.ParentId, Role = model.Role });
            await _repository.SaveChangesAsync();
        }
        else
        {
            result.AddError(ErrorCode.ROLES_ALREADY_EXIST);
        }
        return result;
    }

    public async Task<ModelOrError<bool>> UpdateRoleAsync(RoleEntity role)
    {
        var result = new ModelOrError<bool>
        {
            Model = false
        };

        if (!IsParentInChildrens(role.ParentId, role.Childrens))
        {
            await _repository.UpdateAsync(role, role.Id);
            await _repository.SaveChangesAsync();
            result.Model = true;
        }
        else
        {
            result.AddError(ErrorCode.ROLES_HIERARCHY_RECURSION);
        }
        return result;
    }

    public async Task<List<PermissionEntity>> GetRolePermissionsAsync(RoleEntity role)
    {
        var permissions = new HashSet<PermissionEntity>(role.Permissions);

        var parent = role.ParentRole;
        while (parent != null)
        {
            permissions.UnionWith(parent.Permissions);
            parent = parent.ParentRole;
        }
        return await Task.FromResult(permissions.ToList());
    }

    public async Task AddPermissionsToRoleAsync(RoleEntity role, List<int> permissions)
    {
        var permissionEntities = await _repository.FindAllAsync<PermissionEntity>(x => permissions.Contains(x.Id));

        foreach (PermissionEntity permission in permissionEntities)
        {
            if (role.Permissions.FirstOrDefault(x => x.Id == permission.Id) == null)
            {
                role.Permissions.Add(permission);
            }
        }
    }

    public async Task RemovePermissionsFromRoleAsync(RoleEntity role, List<int> roles)
    {
        var permissionEntities = await _repository.FindAllAsync<PermissionEntity>(x => roles.Contains(x.Id));

        foreach (PermissionEntity permission in permissionEntities)
        {
            if (role.Permissions.FirstOrDefault(x => x.Id == permission.Id) != null)
            {
                role.Permissions.Remove(permission);
            }
        }
    }

    #region support
    private bool IsParentInChildrens(int? parentId, ICollection<RoleEntity> childrens, HashSet<int?>? visited = null)
    {
        bool found = false;

        if (visited == null)
        {
            visited = new HashSet<int?> { parentId };
        }
        foreach (var child in childrens)
        {
            if (visited.Add(child.Id))
            {
                found = IsParentInChildrens(child.ParentId, child.Childrens, visited);
            }
            else
            {
                found = true;
                break;
            }
        }
        return found;
    }
    #endregion
}
