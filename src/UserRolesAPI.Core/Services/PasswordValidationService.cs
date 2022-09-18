using System.Text.RegularExpressions;
using UserRolesAPI.Core.Interfaces;

namespace UserRolesAPI.Core.Services;

public class PasswordValidationService : IPasswordValidationService
{
    public (bool, string) ValidatePassword(string password)
    {
        var errorResult = "";

        var regex = new Regex(@"^(?=.*[a-z]).+$");

        if (!regex.IsMatch(password))
            errorResult = $"{errorResult} Minimum 1 lower case letter.";

        regex = new Regex(@"^(?=.*[A-Z]).+$");

        if (!regex.IsMatch(password))
            errorResult = $"{errorResult} Minimum 1 upper case letter.";

        regex = new Regex(@"^(?=.*\d).+$");

        if (!regex.IsMatch(password))
            errorResult = $"{errorResult} Minimum 1 number.";

        regex = new Regex(@"^(?=.*[-_@$!%*?&=+<>#~|]).+$");

        if (!regex.IsMatch(password))
            errorResult = $"{errorResult} Minimum 1 special symbol.";

        regex = new Regex(@"^.{6,25}$");

        if (!regex.IsMatch(password))
            errorResult = $"{errorResult} Between 6 and 25 characters.";

        if (string.IsNullOrWhiteSpace(errorResult))
            return (true, errorResult);

        errorResult = $"Invalid password.{errorResult}";

        return (false, errorResult);
    }
}
