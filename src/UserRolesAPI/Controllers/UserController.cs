using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserRolesAPI.Core.Entities;
using UserRolesAPI.Core.Extensions.FilterAttributes;
using UserRolesAPI.Core.Helpers;
using UserRolesAPI.Core.Interfaces;
using UserRolesAPI.Core.Models;

namespace UserRolesAPI.Controllers;

[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IHashGenerator _hashGenerator;
    private readonly IPasswordValidationService _passwordValidationService;

    public UserController(IUserService userService, IHashGenerator hashGenerator,
        IPasswordValidationService passwordValidationService)
    {
        _userService = userService;
        _hashGenerator = hashGenerator;
        _passwordValidationService = passwordValidationService;
    }

    [HttpGet(), ValidateModel]
    public IActionResult GetUsers(int? page)
    {
        var users = _userService.GetAllUsers();
        var paginatedUsers = new PaginatedList<UserEntity>(users, page ?? 0, 10);
        return Ok(paginatedUsers);
    }

    [HttpGet("Permissions"), ValidateModel]
    public async Task<IActionResult> GetPermissions(Guid userId)
    {
        var user = await _userService.GetUserAsync(userId);
        return Ok(await _userService.GetUserPermissionsAsync(user));
    }

    [HttpPost("Register"), ValidateModel]
    public async Task<IActionResult> Register([FromBody] UserModel model)
    {
        var passwordValidationResult = _passwordValidationService.ValidatePassword(model.Password);
        if (!passwordValidationResult.Item1)
        {
            return BadRequest(passwordValidationResult.Item2);
        }
        model.Email = model.Email;
        model.Password = _hashGenerator.BCryptHashPassword(model.Password);
        var response = await _userService.RegisterUserAsync(model);
        return response.GetResult();
    }

    [HttpGet("Roles"), ValidateModel]
    public async Task<IActionResult> GetRoles(Guid userId)
    {
        var user = await _userService.GetUserAsync(userId);
        var roles = await _userService.GetUserRolesAsync(user);
        return Ok(roles);
    }

    [HttpPost("Roles/Add"), ValidateModel]
    public async Task<IActionResult> AddRoles([FromBody] UserRolesModel model)
    {
        var user = await _userService.GetUserAsync(model.UserId);
        await _userService.AddRolesToUserAsync(user, model.RoleIds);
        return Ok();
    }
}
