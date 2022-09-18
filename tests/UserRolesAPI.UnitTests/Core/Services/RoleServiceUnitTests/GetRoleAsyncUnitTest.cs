using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using UserRolesAPI.Core.Entities;
using UserRolesAPI.Core.Interfaces.Data;
using UserRolesAPI.Core.Services;

namespace UserRolesAPI.UnitTests.Core.Services.RoleServiceUnitTests;

[TestFixture]
public class GetRoleAsyncUnitTest
{
    private readonly RoleEntity _employeeRole;

    public GetRoleAsyncUnitTest()
    {
        var readPermissions = new PermissionEntity { Permission = "read" };

        _employeeRole = new RoleEntity
        {
            Id = 0,
            ParentId = null,
            Role = "Employee",
            Default = true,
            Permissions = new PermissionEntity[] { readPermissions }
        };
    }

    [Theory, AutoDomainData]
    public async Task Verify_Returned_RoleAsync([Frozen] Mock<IRepository> mockRepository, RoleService sut, int roleId)
    {
        // Arrange
        mockRepository.Setup(x => x.FirstAsync<RoleEntity>(x => x.Id == roleId)).ReturnsAsync(_employeeRole);
        // Act
        var result = await sut.GetRoleAsync(roleId);
        // Assert
        Assert.AreEqual(_employeeRole.Id, result?.Id);
        Assert.AreEqual(_employeeRole.ParentId, result?.ParentId);
        Assert.AreEqual(_employeeRole.Role, result?.Role);
        Assert.AreEqual(_employeeRole.Default, result?.Default);
        Assert.That(_employeeRole.Permissions, Has.All.Matches<PermissionEntity>(e => result.Permissions.Any(
            r => e.Id == r.Id && e.Permission == r.Permission)));
    }
}
