using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using UserRolesAPI.Core.Entities;
using UserRolesAPI.Core.Interfaces;
using UserRolesAPI.Core.Services;

namespace UserRolesAPI.UnitTests.Core.Services.UserServiceUnitTests;

[TestFixture]
public class GetRolePermissionsAsyncUnitTest
{
    private readonly RoleEntity _employeeRole;
    private readonly RoleEntity _managerRole;

    public GetRolePermissionsAsyncUnitTest()
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
            Permissions = new PermissionEntity[] { readPermissions },
            ParentRole = null
        };
        _managerRole = new RoleEntity
        {
            Id = 1,
            ParentId = 0,
            Role = "Manager",
            Default = false,
            Permissions = new PermissionEntity[] { addpermissions, updatePermissions, deletePermissions },
            ParentRole = _employeeRole
        };
    }

    [Theory, AutoDomainData]
    public async Task Verify_Role_Permissions_Without_ParentAsync(RoleService sut, RoleEntity role)
    {
        // Arrange
        var expectedPermissions = _employeeRole.Permissions;
        // Act
        var result = await sut.GetRolePermissionsAsync(_employeeRole);
        // Assert
        Assert.That(expectedPermissions, Has.All.Matches<PermissionEntity>(e => result.Any(r => e.Id == r.Id && e.Permission == r.Permission)));
    }

    [Theory, AutoDomainData]
    public async Task Verify_Role_Permissions_With_ParentAsync(RoleService sut, RoleEntity role)
    {
        // Arrange
        var expectedPermissions = _managerRole.Permissions.Union(_employeeRole.Permissions);
        // Act
        var result = await sut.GetRolePermissionsAsync(_managerRole);
        // Assert
        Assert.That(expectedPermissions, Has.All.Matches<PermissionEntity>(e => result.Any(r => e.Id == r.Id && e.Permission == r.Permission)));
    }
}
