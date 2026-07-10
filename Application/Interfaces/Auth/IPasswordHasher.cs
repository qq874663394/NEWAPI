namespace Application.Interfaces.Auth;

/// <summary>
/// 密码哈希器（接口在 Application 层，实现在 Infrastructure 层，遵循依赖倒置）
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// 对密码进行哈希加密
    /// </summary>
    string HashPassword(string password);

    /// <summary>
    /// 验证密码是否与哈希值匹配
    /// </summary>
    bool VerifyPassword(string password, string storedHash);
}
