namespace UserRolesAPI.Core.Interfaces;

public interface IHashGenerator
{
    string HMACSHA256Password(string stringToHash);
    string HMACSHA256(string rawString, string secretKey, bool generateSalt = false);
    bool HMACSHA256Verify(string hashStr, string rawString, bool containsSalt);
    bool HMACSHA256Verify(string hashStr, string secretKey, string rawString, bool containsSalt);
    string HMACSHA256(string rawString, bool generateSalt = false);
    string BCryptHashPassword(string password);
    bool BCrypVerifyPassword(string password, string hash);
    string SHA256(string rawString);
    string SHA256Hex(string rawString);
}
