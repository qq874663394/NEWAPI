namespace Application.Auth.DTO;

/// <summary>
/// Provider内部使用（避免污染Controller）
/// </summary>
public class AuthenticationRequest
{
    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string AuthType { get; set; } = string.Empty;
}