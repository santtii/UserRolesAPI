using UserRolesAPI.Core.Interfaces.Settings;

namespace UserRolesAPI.Infrastructure.Settings;

public class DbSettings : IDbSettings
{
    public string DefaultConnection { get; set; } = String.Empty;
}
