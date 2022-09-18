using System.Collections.ObjectModel;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using UserRolesAPI.Core.Entities;
using UserRolesAPI.Core.Interfaces.Data;
using UserRolesAPI.Core.Services;
using UserRolesAPI.SharedKernel;

namespace UserRolesAPI.UnitTests.Core.Services.RoleServiceUnitTests;

[TestFixture]
public class UpdateRoleAsyncUnitTest
{
    private readonly RoleEntity _employeeRole;
    private readonly RoleEntity _managerRole;

    public UpdateRoleAsyncUnitTest()
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
            Permissions = new PermissionEntity[] { addpermissions, updatePermissions, deletePermissions },
            ParentRole = _employeeRole
        };
        _employeeRole.Childrens = new RoleEntity[] { _managerRole };
    }

    [SetUp]
    public void Init()  // setup overriden fields after each test
    {
        _employeeRole.ParentId = null;
        _managerRole.Childrens = new Collection<RoleEntity>();
    }

    [Theory, AutoDomainData]
    public async Task Verify_Updated_RoleAsync(RoleService sut)
    {
        // Act
        var result = await sut.UpdateRoleAsync(_employeeRole);
        // Assert
        Assert.AreEqual(true, result.Model);
    }

    [Theory, AutoDomainData]
    public async Task Verify_That_Updated_Role_Is_SavedAsync([Frozen] Mock<IRepository> mockRepository, RoleService sut)
    {
        // Act
        var result = await sut.UpdateRoleAsync(_employeeRole);
        // Assert
        mockRepository.Verify(x => x.SaveChangesAsync(true), Times.Once);
    }

    [Theory, AutoDomainData]
    public async Task Verify_Error_With_Recursive_ParentAsync(RoleService sut)
    {
        // Arrange
        _employeeRole.ParentId = 1;
        _managerRole.Childrens = new RoleEntity[] { _employeeRole };
        // Act
        var result = await sut.UpdateRoleAsync(_employeeRole);
        // Assert
        Assert.AreEqual((int)ErrorCode.ROLES_HIERARCHY_RECURSION, result.Error);
    }
}
