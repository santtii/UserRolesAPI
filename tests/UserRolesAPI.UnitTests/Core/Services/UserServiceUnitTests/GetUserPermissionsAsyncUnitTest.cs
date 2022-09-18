using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using UserRolesAPI.Core.Entities;
using UserRolesAPI.Core.Interfaces;
using UserRolesAPI.Core.Services;

namespace UserRolesAPI.UnitTests.Core.Services.UserServiceUnitTests;

[TestFixture]
public class GetUserPermissionsAsyncUnitTest
{
    private readonly RoleEntity _employeeRole;
    private readonly RoleEntity _managerRole;

    public GetUserPermissionsAsyncUnitTest()
    {
        var readPermissions = new PermissionEntity { Permission = "read" };
        var addpermissions = new PermissionEntity { Permission = "add" };
        var updatePermissions = new PermissionEntity { Permission = "update" };
        var deletePermissions = new PermissionEntity { Permission = "delete" };

        _employeeRole = new RoleEntity
        {
            Id = 0,
            ParentId = null,
            Role = "Employee",
            Default = true,
            Permissions = new PermissionEntity[] { readPermissions }
        };
        _managerRole = new RoleEntity
        {
            Id = 1,
            ParentId = 0,
            Role = "Manager",
            Default = false,
            Permissions = new PermissionEntity[] { addpermissions, updatePermissions, deletePermissions }
        };
    }

    [Theory, AutoDomainData]
    public async Task Verify_Role_Permissions_Without_ParentAsync([Frozen] Mock<IRoleService> mockRoleService, UserService sut, UserEntity user)
    {
        // Arrange
        user.Roles = new List<RoleEntity>() { _employeeRole };
        var expectedEmployeePermission = _employeeRole.Permissions.ToList();
        mockRoleService.Setup(x => x.GetRolePermissionsAsync(_employeeRole)).ReturnsAsync(expectedEmployeePermission);
        // Act
        var result = await sut.GetUserPermissionsAsync(user);
        // Assert
        Assert.That(expectedEmployeePermission, Has.All.Matches<PermissionEntity>(e => result.Any(r => e.Id == r.Id && e.Permission == r.Permission)));
    }

    [Theory, AutoDomainData]
    public async Task Verify_Role_Permissions_With_ParentAsync([Frozen] Mock<IRoleService> mockRoleService, UserService sut, UserEntity user)
    {
        // Arrange
        user.Roles = new List<RoleEntity>() { _managerRole };
        var expectedManagerPermission = _managerRole.Permissions.Union(_employeeRole.Permissions).ToList();
        mockRoleService.Setup(x => x.GetRolePermissionsAsync(_managerRole)).ReturnsAsync(expectedManagerPermission);
        // Act
        var result = await sut.GetUserPermissionsAsync(user);
        // Assert
        Assert.That(expectedManagerPermission, Has.All.Matches<PermissionEntity>(e => result.Any(r => e.Id == r.Id && e.Permission == r.Permission)));
    }
}
