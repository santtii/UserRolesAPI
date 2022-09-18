using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserRolesAPI.Core.Extensions.FilterAttributes;
using UserRolesAPI.Core.Interfaces;
using UserRolesAPI.Core.Models;

namespace UserRolesAPI.Controllers;

[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class RoleController : ControllerBase
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetRolesAsync()
    {
        return Ok(await _roleService.GetRolesAsync());
    }

    [HttpPost("Add"), ValidateModel]
    public async Task<IActionResult> AddRole([FromBody] RoleModel model)
    {
        return Ok(await _roleService.AddRoleAsync(model));
    }

    [HttpGet("Permissions"), ValidateModel]
    public async Task<IActionResult> GetPermissions(int roleId)
    {
        var role = await _roleService.GetRoleAsync(roleId);
        var permissions = await _roleService.GetRolePermissionsAsync(role);
        return Ok(permissions);
    }

    [HttpPost("Permissions/Add"), ValidateModel]
    public async Task<IActionResult> AddPermissions([FromBody] RolePermissionsModel model)
    {
        var role = await _roleService.GetRoleAsync(model.RoleId);
        await _roleService.AddPermissionsToRoleAsync(role, model.PermissionIds);
        return Ok();
    }
}
