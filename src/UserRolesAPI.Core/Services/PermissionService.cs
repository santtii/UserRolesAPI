using UserRolesAPI.Core.Entities;
using UserRolesAPI.Core.Interfaces;
using UserRolesAPI.Core.Interfaces.Data;

namespace UserRolesAPI.Core.Services;
public class PermissionService : IPermissionService
{
    private readonly IRepository _repository;

    public PermissionService(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<PermissionEntity>> GetPermissionsAsync()
    {
        return await _repository.GetAllAsync<PermissionEntity>();
    }
}
