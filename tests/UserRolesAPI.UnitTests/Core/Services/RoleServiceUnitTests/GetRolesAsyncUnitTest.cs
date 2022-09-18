using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using UserRolesAPI.Core.Entities;
using UserRolesAPI.Core.Interfaces.Data;
using UserRolesAPI.Core.Services;

namespace UserRolesAPI.UnitTests.Core.Services.RoleServiceUnitTests;

[TestFixture]
public class GetRolesAsyncUnitTest
{
    private readonly List<RoleEntity> _allRoles;

    public GetRolesAsyncUnitTest()
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
        _allRoles = new List<RoleEntity> { employeeRole, managerRole };
    }

    [Theory, AutoDomainData]
    public async Task Verify_Returned_RolesAsync([Frozen] Mock<IRepository> mockRepository, RoleService sut, int roleId)
    {
        // Arrange
        mockRepository.Setup(x => x.GetAllAsync<RoleEntity>()).ReturnsAsync(_allRoles);
        // Act
        var result = await sut.GetRolesAsync();
        // Assert
        Assert.That(_allRoles, Has.All.Matches<RoleEntity>(e => result.Any(r => e.Id == r.Id && e.ParentId == r.ParentId
            && e.Role == r.Role && e.Default == r.Default)));
    }
}
