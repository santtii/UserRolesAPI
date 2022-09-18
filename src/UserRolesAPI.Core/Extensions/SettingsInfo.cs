namespace UserRolesAPI.Core.Extensions;

public class SettingsInfo
{
    public readonly string SectionName;
    public readonly string EnvironmentPrefix;

    public SettingsInfo(string sectionName, string environmentPrefix = null)
    {
        SectionName = sectionName;
        EnvironmentPrefix = environmentPrefix;
    }
}
