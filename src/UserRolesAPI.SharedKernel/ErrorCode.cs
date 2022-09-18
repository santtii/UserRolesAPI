using System.ComponentModel;

namespace UserRolesAPI.SharedKernel;

public enum ErrorCode
{
    [Description("Email already registered")]
    EMAIL_ALREADY_REGISTERED = 10,
    [Description("Role already exist")]
    ROLES_ALREADY_EXIST = 11,
    [Description("Recursive role hierarchy detected")]
    ROLES_HIERARCHY_RECURSION = 12,
}
