using System.ComponentModel.DataAnnotations;

namespace UserRolesAPI.Core.Models;

public class CredentialsModel
{
    [Required]
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
