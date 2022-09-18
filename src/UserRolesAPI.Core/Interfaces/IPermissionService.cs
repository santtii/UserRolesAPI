using UserRolesAPI.Core.Entities;

namespace UserRolesAPI.Core.Interfaces;
public interface IPermissionService
{
    Task<List<PermissionEntity>> GetPermissionsAsync();
}
