using Application.Interfaces.Auth;
using Infrastructure.Authentication.Security;

namespace Infrastructure.Authentication.Security;

/// <summary>
/// 密码哈希器实现（适配 CryptHelper）
/// </summary>
public class PasswordHasher : IPasswordHasher
{
    /// <inheritdoc />
    public string HashPassword(string password)
        => CryptHelper.HashPassword(password);

    /// <inheritdoc />
    public bool VerifyPassword(string password, string storedHash)
        => CryptHelper.VerifyPassword(password, storedHash);
}
