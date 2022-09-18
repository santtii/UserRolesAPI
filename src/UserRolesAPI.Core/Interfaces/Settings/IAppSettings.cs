namespace UserRolesAPI.Core.Interfaces.Settings;

public interface IAppSettings
{
    public string Domain { get; set; }
    public string SecurityKey { get; set; }
}
