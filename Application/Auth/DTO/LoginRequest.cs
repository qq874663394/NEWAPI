namespace Application.Auth.DTO;

public class LoginRequest
{
    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Password / Ldap / Apo / OAuth / Windows
    /// </summary>
    public string AuthType { get; set; } = "Password";
}