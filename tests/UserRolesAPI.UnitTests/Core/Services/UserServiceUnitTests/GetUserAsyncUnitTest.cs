using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using UserRolesAPI.Core.Entities;
using UserRolesAPI.Core.Interfaces.Data;
using UserRolesAPI.Core.Services;

namespace UserRolesAPI.UnitTests.Core.Services.UserServiceUnitTests;

[TestFixture]
public class GetUserAsyncUnitTest
{
    private readonly UserEntity _user;

    public GetUserAsyncUnitTest()
    {
        _user = new UserEntity
        {
            Name = "Test Name 1",
            Email = "email_1@test:com",
            Departament = "Departament Test 1",
            Password = "test-password@1"
        };
    }

    [Theory, AutoDomainData]
    public async Task Verify_Returned_User_By_IdAsync([Frozen] Mock<IRepository> mockRepository, UserService sut, Guid userId)
    {
        // Arrange
        mockRepository.Setup(x => x.FirstAsync<UserEntity>(x => x.Id == userId)).ReturnsAsync(_user);
        // Act
        var result = await sut.GetUserAsync(userId);
        // Assert
        Assert.AreEqual(_user.Id, result?.Id);
        Assert.AreEqual(_user.Name, result?.Name);
        Assert.AreEqual(_user.Email, result?.Email);
        Assert.AreEqual(_user.Departament, result?.Departament);
        Assert.AreEqual(_user.Password, result?.Password);
    }

    [Theory, AutoDomainData]
    public async Task Verify_Returned_User_By_EmailAsync([Frozen] Mock<IRepository> mockRepository, UserService sut, string email)
    {
        // Arrange
        mockRepository.Setup(x => x.FirstAsync<UserEntity>(x => x.Email == email)).ReturnsAsync(_user);
        // Act
        var result = await sut.GetUserAsync(email);
        // Assert
        Assert.AreEqual(_user.Id, result?.Id);
        Assert.AreEqual(_user.Name, result?.Name);
        Assert.AreEqual(_user.Email, result?.Email);
        Assert.AreEqual(_user.Departament, result?.Departament);
        Assert.AreEqual(_user.Password, result?.Password);
    }
}
