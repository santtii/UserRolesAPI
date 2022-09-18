using UserRolesAPI.Core.Extensions;

namespace UserRolesAPI.Core.Constant;

public class SettingsConstants
{
    public static readonly SettingsInfo DbSettings = new SettingsInfo("ConnectionStrings", "DB");
    public static readonly SettingsInfo AppSettings = new SettingsInfo("AppSettings", "APS");
}
