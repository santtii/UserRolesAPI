using System.Linq.Expressions;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using UserRolesAPI.Core.Entities;
using UserRolesAPI.Core.Interfaces.Data;
using UserRolesAPI.Core.Services;

namespace UserRolesAPI.UnitTests.Core.Services.UserServiceUnitTests;

[TestFixture]
public class AddRolesToUserAsyncUnitTest
{
    private readonly List<RoleEntity> _expectedRoles;
    private readonly List<int> _rolesToAdd;

    private readonly UserEntity _user;

    public AddRolesToUserAsyncUnitTest()
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
            Roles = new List<RoleEntity> { employeeRole }
        };
        _expectedRoles = new List<RoleEntity> { employeeRole, managerRole };
        _rolesToAdd = _expectedRoles.Select(x => x.Id).ToList();
    }

    [Theory, AutoDomainData]
    public async Task Verify_Correct_Roles_AssignationAsync([Frozen] Mock<IRepository> mockRepository, UserService sut)
    {
        // Arrange
        mockRepository.Setup(x => x.FindAllAsync(It.IsAny<Expression<Func<RoleEntity, bool>>>())).ReturnsAsync(_expectedRoles);
        // Act
        await sut.AddRolesToUserAsync(_user, _rolesToAdd);
        // Assert
        Assert.That(_expectedRoles, Has.All.Matches<RoleEntity>(e => _user.Roles.Any(r => e.Id == r.Id
            && e.ParentId == r.ParentId && e.Role == r.Role && e.Default == r.Default)));
    }

    [Theory, AutoDomainData]
    public async Task Verify_That_Added_Roles_Are_SavedAsync([Frozen] Mock<IRepository> mockRepository, UserService sut)
    {
        // Arrange
        mockRepository.Setup(x => x.FindAllAsync(It.IsAny<Expression<Func<RoleEntity, bool>>>())).ReturnsAsync(_expectedRoles);
        // Act
        await sut.AddRolesToUserAsync(_user, _rolesToAdd);
        // Assert
        mockRepository.Verify(x => x.SaveChangesAsync(true), Times.Once);
    }
}
