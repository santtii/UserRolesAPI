using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using UserRolesAPI.Core.Entities;
using UserRolesAPI.Core.Interfaces.Data;
using UserRolesAPI.Core.Services;

namespace UserRolesAPI.UnitTests.Core.Services.PermissionServiceUnitTests;

[TestFixture]
public class GetPermissionsAsyncUnitTest
{
    private readonly List<PermissionEntity> _allPermissions;

    public GetPermissionsAsyncUnitTest()
    {
        var readPermissions = new PermissionEntity { Id = 0, Permission = "read" };
        var addpermissions = new PermissionEntity { Id = 1, Permission = "add" };
        var updatePermissions = new PermissionEntity { Id = 2, Permission = "update" };
        var deletePermissions = new PermissionEntity { Id = 3, Permission = "delete" };

        _allPermissions = new List<PermissionEntity> { readPermissions, addpermissions, updatePermissions, deletePermissions };
    }

    [Theory, AutoDomainData]
    public async Task Verify_Returned_RolesAsync([Frozen] Mock<IRepository> mockRepository, PermissionService sut)
    {
        // Arrange
        mockRepository.Setup(x => x.GetAllAsync<PermissionEntity>()).ReturnsAsync(_allPermissions);
        // Act
        var result = await sut.GetPermissionsAsync();
        // Assert
        Assert.That(_allPermissions, Has.All.Matches<PermissionEntity>(e => result.Any(r => e.Id == r.Id && e.Permission == r.Permission)));
    }
}
