namespace UserRolesAPI.Core.Interfaces;

public interface IPasswordValidationService
{
    (bool, string) ValidatePassword(string password);
}
