namespace Application.DTO.Auth;

/// <summary>
/// Token 刷新请求
/// </summary>
public class RefreshRequest
{
    /// <summary>
    /// 刷新令牌
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;
}
