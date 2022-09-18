using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using UserRolesAPI.Core.Entities;
using UserRolesAPI.Core.Interfaces.Data;
using UserRolesAPI.Core.Services;

namespace UserRolesAPI.UnitTests.Core.Services.UserServiceUnitTests;

[TestFixture]
public class UpdateUserAsyncUnitTest
{
    [Theory, AutoDomainData]
    public async Task Verify_That_User_Is_UpdatedAsync([Frozen] Mock<IRepository> mockRepository, UserService sut, UserEntity user)
    {
        // Act
        await sut.UpdateUserAsync(user);
        // Assert
        mockRepository.Verify(x => x.UpdateAsync(user, user.Id, It.IsAny<string>()), Times.Once);
        mockRepository.Verify(x => x.SaveChangesAsync(true), Times.Once);
    }
}
