using UserRolesAPI.Core.Entities;
using UserRolesAPI.Core.Interfaces;
using UserRolesAPI.Core.Interfaces.Data;
using UserRolesAPI.Core.Models;
using UserRolesAPI.SharedKernel;

namespace UserRolesAPI.Core.Services;

public class UserService : IUserService
{
    private readonly IRepository _repository;
    private readonly IRoleService _roleService;

    public UserService(IRepository repository, IRoleService roleService)
    {
        _repository = repository;
        _roleService = roleService;
    }

    public IQueryable<UserEntity> GetAllUsers()
    {
        return _repository.GetAll<UserEntity>();
    }

    public async Task<UserEntity> GetUserAsync(Guid userId)
    {
        return await _repository.FirstAsync<UserEntity>(x => x.Id == userId);
    }

    public async Task<UserEntity> GetUserAsync(string email)
    {
        return await _repository.FirstAsync<UserEntity>(x => x.Email == email);
    }

    public async Task<List<PermissionEntity>> GetUserPermissionsAsync(UserEntity user)
    {
        var permissions = new HashSet<PermissionEntity>();

        foreach (RoleEntity role in user.Roles)
        {
            permissions.UnionWith(await _roleService.GetRolePermissionsAsync(role));
        }
        return await Task.FromResult(permissions.ToList());
    }

    public async Task<ModelOrError<UserEntity>> RegisterUserAsync(UserModel model)
    {
        var result = new ModelOrError<UserEntity>();

        var user = await _repository.FirstAsync<UserEntity>(x => x.Email == model.Email.ToLower());
        if (user == null)
        {
            result.Model = await _repository.AddAsync((UserEntity)model);
            if (result.Model != null)
            {
                var defaultRoles = await _repository.FindAllAsync<RoleEntity>(x => x.Default == true);
                if (defaultRoles != null)
                {
                    await AddRolesToUserAsync(result.Model, defaultRoles.Select(x => x.Id).ToList());
                }
                await _repository.SaveChangesAsync();
            }
        }
        else
        {
            result.AddError(ErrorCode.EMAIL_ALREADY_REGISTERED);
        }
        return result;
    }

    public async Task UpdateUserAsync(UserEntity user)
    {
        await _repository.UpdateAsync(user, user.Id);
        await _repository.SaveChangesAsync();
    }

    public async Task<List<RoleEntity>> GetUserRolesAsync(UserEntity user)
    {
        return await Task.FromResult(user.Roles.ToList());
    }

    public async Task AddRolesToUserAsync(UserEntity user, List<int> roles)
    {
        var rolesEntities = await _repository.FindAllAsync<RoleEntity>(x => roles.Contains(x.Id));

        foreach (RoleEntity role in rolesEntities)
        {
            if (user.Roles.FirstOrDefault(x => x.Id == role.Id) == null)
            {
                user.Roles.Add(role);
            }
        }
        await _repository.SaveChangesAsync();
    }

    public async Task RemoveRolesFromUserAsync(UserEntity user, List<int> roles)
    {
        var rolesEntities = await _repository.FindAllAsync<RoleEntity>(x => roles.Contains(x.Id));

        foreach (RoleEntity role in rolesEntities)
        {
            if (user.Roles.FirstOrDefault(x => x.Id == role.Id) != null)
            {
                user.Roles.Remove(role);
            }
        }
        await _repository.SaveChangesAsync();
    }
}
