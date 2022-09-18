using UserRolesAPI.Core.Interfaces.Settings;

namespace UserRolesAPI.Infrastructure.Settings;

public class AppSettings : IAppSettings
{
    public string Domain { get; set; } = String.Empty;
    public string SecurityKey { get; set; } = String.Empty;
}
