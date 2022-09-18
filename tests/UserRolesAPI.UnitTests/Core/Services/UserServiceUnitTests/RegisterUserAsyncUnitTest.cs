using System.Linq.Expressions;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using UserRolesAPI.Core.Entities;
using UserRolesAPI.Core.Interfaces.Data;
using UserRolesAPI.Core.Models;
using UserRolesAPI.Core.Services;
using UserRolesAPI.SharedKernel;

namespace UserRolesAPI.UnitTests.Core.Services.UserServiceUnitTests;

[TestFixture]
public class RegisterUserAsyncUnitTest
{
    private readonly RoleEntity _employeeRole;
    private readonly List<RoleEntity> _userRoles;

    private readonly UserEntity _user;

    public RegisterUserAsyncUnitTest()
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
        _user = new UserEntity
        {
            Name = "Test Name 1",
            Email = "email_1@test:com",
            Departament = "Departament Test 1",
            Password = "test-password@1",
        };
        _userRoles = new List<RoleEntity> { _employeeRole };
    }

    [SetUp]
    public void Init()  // setup overriden fields after each test
    {
        _user.Roles.Clear();
    }

    [Theory, AutoDomainData]
    public async Task Verify_User_With_Default_RoleAsync([Frozen] Mock<IRepository> mockRepository, UserService sut, UserModel model)
    {
        // Arrange
        mockRepository.Setup(x => x.FirstAsync<UserEntity>(x => x.Email == model.Email.ToLower())).ReturnsAsync((UserEntity)null);
        mockRepository.Setup(x => x.AddAsync(It.IsAny<UserEntity>(), It.IsAny<string>())).ReturnsAsync(_user);
        mockRepository.Setup(x => x.FindAllAsync<RoleEntity>(x => x.Default == true)).ReturnsAsync(_userRoles);
        mockRepository.Setup(x => x.FindAllAsync(It.IsAny<Expression<Func<RoleEntity, bool>>>())).ReturnsAsync(_userRoles);
        // Act
        var result = await sut.RegisterUserAsync(model);
        // Assert
        Assert.AreEqual(_user.Id, result.Model?.Id);
        Assert.AreEqual(_user.Name, result.Model?.Name);
        Assert.AreEqual(_user.Email, result.Model?.Email);
        Assert.AreEqual(_user.Departament, result.Model?.Departament);
        Assert.AreEqual(_user.Password, result.Model?.Password);
        Assert.That(_user.Roles, Has.All.Matches<RoleEntity>(e => result.Model.Roles.Any(r => e.Id == r.Id
            && e.ParentId == r.ParentId && e.Role == r.Role && e.Default == r.Default)));
    }

    [Theory, AutoDomainData]
    public async Task Verify_User_Without_Default_RoleAsync([Frozen] Mock<IRepository> mockRepository, UserService sut, UserModel model)
    {
        // Arrange
        mockRepository.Setup(x => x.FirstAsync<UserEntity>(x => x.Email == model.Email.ToLower())).ReturnsAsync((UserEntity)null);
        mockRepository.Setup(x => x.AddAsync(It.IsAny<UserEntity>(), It.IsAny<string>())).ReturnsAsync(_user);
        mockRepository.Setup(x => x.FindAllAsync<RoleEntity>(x => x.Default == true)).ReturnsAsync((List<RoleEntity>)null);
        mockRepository.Setup(x => x.FindAllAsync(It.IsAny<Expression<Func<RoleEntity, bool>>>())).ReturnsAsync((List<RoleEntity>)null);
        // Act
        var result = await sut.RegisterUserAsync(model);
        // Assert
        Assert.AreEqual(_user.Id, result.Model?.Id);
        Assert.AreEqual(_user.Name, result.Model?.Name);
        Assert.AreEqual(_user.Email, result.Model?.Email);
        Assert.AreEqual(_user.Departament, result.Model?.Departament);
        Assert.AreEqual(_user.Password, result.Model?.Password);
        Assert.IsEmpty(_user.Roles);
    }

    [Theory, AutoDomainData]
    public async Task Verify_That_User_Is_SavedAsync([Frozen] Mock<IRepository> mockRepository, UserService sut, UserModel model)
    {
        // Arrange
        mockRepository.Setup(x => x.FirstAsync<UserEntity>(x => x.Email == model.Email.ToLower())).ReturnsAsync((UserEntity)null);
        mockRepository.Setup(x => x.AddAsync(It.IsAny<UserEntity>(), It.IsAny<string>())).ReturnsAsync(_user);
        mockRepository.Setup(x => x.FindAllAsync<RoleEntity>(x => x.Default == true)).ReturnsAsync((List<RoleEntity>)null);
        mockRepository.Setup(x => x.FindAllAsync(It.IsAny<Expression<Func<RoleEntity, bool>>>())).ReturnsAsync((List<RoleEntity>)null);
        // Act
        var result = await sut.RegisterUserAsync(model);
        // Assert
        mockRepository.Verify(x => x.SaveChangesAsync(true), Times.Once);
    }

    [Theory, AutoDomainData]
    public async Task Verify_Error_Code_When_User_Already_RegisteredAsync([Frozen] Mock<IRepository> mockRepository, UserService sut, UserModel model)
    {
        // Arrange
        mockRepository.Setup(x => x.FirstAsync<UserEntity>(x => x.Email == model.Email.ToLower())).ReturnsAsync(_user);
        // Act
        var result = await sut.RegisterUserAsync(model);
        // Assert
        Assert.AreEqual((int)ErrorCode.EMAIL_ALREADY_REGISTERED, result.Error);
    }
}
