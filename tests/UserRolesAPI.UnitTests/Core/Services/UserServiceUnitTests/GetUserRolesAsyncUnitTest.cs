using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using UserRolesAPI.Core.Entities;
using UserRolesAPI.Core.Interfaces;
using UserRolesAPI.Core.Services;

namespace UserRolesAPI.UnitTests.Core.Services.UserServiceUnitTests;

[TestFixture]
public class GetUserRolesAsyncUnitTest
{
    private readonly UserEntity _user;

    public GetUserRolesAsyncUnitTest()
    {
        var readPermissions = new PermissionEntity { Permission = "read" };
        var addpermissions = new PermissionEntity { Permission = "add" };
        var updatePermissions = new PermissionEntity { Permission = "update" };
        var deletePermissions = new PermissionEntity { Permission = "delete" };

        var employeeRole = new RoleEntity
        {
            Id = 0,
            ParentId = null,
            Role = "Employee",
            Default = true,
            Permissions = new PermissionEntity[] { readPermissions }
        };
        var managerRole = new RoleEntity
        {
            Id = 1,
            ParentId = 0,
            Role = "Manager",
            Default = false,
            Permissions = new PermissionEntity[] { addpermissions, updatePermissions, deletePermissions }
        };
        _user = new UserEntity
        {
            Name = "Test Name 1",
            Email = "email_1@test:com",
            Departament = "Departament Test 1",
            Password = "test-password@1",
            Roles = new List<RoleEntity> { employeeRole, managerRole }
        };
    }

    [Theory, AutoDomainData]
    public async Task Verify_Returned_RolesAsync([Frozen] Mock<IRoleService> mockRoleService, UserService sut)
    {
        // Act
        var result = await sut.GetUserRolesAsync(_user);
        // Assert
        Assert.That(_user.Roles, Has.All.Matches<RoleEntity>(e => result.Any(r => e.Id == r.Id && e.ParentId == r.ParentId
            && e.Role == r.Role && e.Default == r.Default)));
    }
}
