namespace Application.DTO.Auth;

public class LoginResult
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = default!;

    public DateTime ExpireAt { get; set; }

    /// <summary>
    /// 用户唯一标识（你说的是 Code）
    /// </summary>
    public Guid? UserCode { get; set; } = Guid.Empty;

    public string UserName { get; set; } = string.Empty;
}