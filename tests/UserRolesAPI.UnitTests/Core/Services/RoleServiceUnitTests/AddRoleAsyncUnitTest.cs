using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using UserRolesAPI.Core.Entities;
using UserRolesAPI.Core.Interfaces.Data;
using UserRolesAPI.Core.Models;
using UserRolesAPI.Core.Services;
using UserRolesAPI.SharedKernel;

namespace UserRolesAPI.UnitTests.Core.Services.RoleServiceUnitTests;

[TestFixture]
public class AddRoleAsyncUnitTest
{
    private readonly RoleEntity _employeeRole;

    public AddRoleAsyncUnitTest()
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
    public async Task Verify_Added_RoleAsync([Frozen] Mock<IRepository> mockRepository, RoleService sut, RoleModel model)
    {
        // Arrange
        mockRepository.Setup(x => x.FirstAsync<RoleEntity>(x => x.Role == model.Role)).ReturnsAsync((RoleEntity)null);
        mockRepository.Setup(x => x.AddAsync(It.IsAny<RoleEntity>(), It.IsAny<string>())).ReturnsAsync(_employeeRole);
        // Act
        var result = await sut.AddRoleAsync(model);
        // Assert
        Assert.AreEqual(_employeeRole.ParentId, result.Model?.ParentId);
        Assert.AreEqual(_employeeRole.Role, result.Model?.Role);
        Assert.AreEqual(_employeeRole.Default, result.Model?.Default);
    }

    [Theory, AutoDomainData]
    public async Task Verify_That_Added_Role_Is_SavedAsync([Frozen] Mock<IRepository> mockRepository, RoleService sut, RoleModel model)
    {
        // Arrange
        mockRepository.Setup(x => x.FirstAsync<RoleEntity>(x => x.Role == model.Role)).ReturnsAsync((RoleEntity)null);
        mockRepository.Setup(x => x.AddAsync(It.IsAny<RoleEntity>(), It.IsAny<string>())).ReturnsAsync(_employeeRole);
        // Act
        var result = await sut.AddRoleAsync(model);
        // Assert
        mockRepository.Verify(x => x.SaveChangesAsync(true), Times.Once);
    }

    [Theory, AutoDomainData]
    public async Task Verify_Error_If_Role_Already_ExistAsync([Frozen] Mock<IRepository> mockRepository, RoleService sut, RoleModel model)
    {
        // Arrange
        mockRepository.Setup(x => x.FirstAsync<RoleEntity>(x => x.Role == model.Role)).ReturnsAsync(_employeeRole);
        // Act
        var result = await sut.AddRoleAsync(model);
        // Assert
        Assert.AreEqual((int)ErrorCode.ROLES_ALREADY_EXIST, result.Error);
    }
}
