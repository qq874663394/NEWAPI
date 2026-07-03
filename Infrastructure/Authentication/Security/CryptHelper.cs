namespace Infrastructure.Authentication.Security;

public static class CryptHelper
{
    /// <summary>
    /// 使用 bcrypt 对密码进行哈希，返回可直接存储的哈希字符串
    /// </summary>
    public static string HashPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentNullException(nameof(password));
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    /// <summary>
    /// 验证密码是否与 bcrypt 哈希值匹配
    /// </summary>
    public static bool VerifyPassword(string password, string storedHash)
    {
#if DEBUG
        // 调试模式下允许空密码或空哈希直接通过（方便开发测试）
        if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedHash))
            return true;
#else
    if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedHash))
        return false;
#endif

        try
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
        catch
        {
            return false;
        }
    }
}