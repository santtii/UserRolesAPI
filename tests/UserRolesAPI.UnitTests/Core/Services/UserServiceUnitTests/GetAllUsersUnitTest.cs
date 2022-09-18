using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using UserRolesAPI.Core.Entities;
using UserRolesAPI.Core.Interfaces.Data;
using UserRolesAPI.Core.Services;

namespace UserRolesAPI.UnitTests.Core.Services.UserServiceUnitTests;

[TestFixture]
public class GetAllUsersUnitTest
{
    private readonly List<UserEntity> _users;

    public GetAllUsersUnitTest()
    {
        _users = new List<UserEntity>
        {
            new UserEntity
            {
                Name = "Test Name 1",
                Email = "email_1@test:com",
                Departament = "Departament Test 1",
                Password = "test-password@1"
            },
            new UserEntity
            {
                Name = "Test Name 2",
                Email = "email_2@test:com",
                Departament = "Departament Test 2",
                Password = "test-password@2"
            }
        };
    }

    [Theory, AutoDomainData]
    public void Verify_Returned_Users([Frozen] Mock<IRepository> mockRepository, UserService sut)
    {
        // Arrange
        mockRepository.Setup(x => x.GetAll<UserEntity>()).Returns(_users.AsQueryable());
        // Act
        var result = sut.GetAllUsers();
        // Assert
        Assert.That(_users, Has.All.Matches<UserEntity>(e => result.Any(r => e.Id == r.Id && e.Name == r.Name
            && e.Email == r.Email && e.Departament == r.Departament && e.Password == r.Password)));
    }
}
