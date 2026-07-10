using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.DTO.Auth;
using Application.Interfaces.Auth;
using Application.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Authentication.Security;

public class JwtTokenService : ITokenService
{
    private readonly JwtOptions _options;

    public JwtTokenService(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    // =========================
    // 创建 Token
    // =========================
    public JwtTokenResult CreateToken(Guid userCode, string userName = "")
    {
        // ----- AccessToken（短时效，例如 30 分钟） -----
        var accessExpires = DateTime.UtcNow.AddMinutes(_options.ExpireMinutes);
        var accessClaims = new[]
        {
        new Claim(ClaimTypes.NameIdentifier, userCode.ToString()),
        new Claim(ClaimTypes.Name, userName ?? string.Empty)
    };
        var accessToken = BuildToken(accessClaims, accessExpires);

        // ----- RefreshToken（长时效，例如 7 天） -----
        var refreshExpires = DateTime.UtcNow.AddDays(7);
        var refreshClaims = new[]
        {
        new Claim(ClaimTypes.NameIdentifier, userCode.ToString()),
        // 可加一个专用 claim 标识这是刷新令牌
        new Claim("token_type", "refresh")
    };
        var refreshToken = BuildToken(refreshClaims, refreshExpires);

        return new JwtTokenResult
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpireAt = accessExpires
        };
    }

    // 抽取生成 JWT 的公共方法
    private string BuildToken(Claim[] claims, DateTime expires)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: creds);
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    // ========== 验证 Access Token（仅验证并返回用户ID） ==========
    public Guid? ValidateToken(string token)
    {
        var principal = ValidateTokenInternal(token, validateLifetime: true);
        if (principal == null) return null;

        var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(userId, out var id) ? id : null;
    }

    // ========== 内部验证方法 ==========
    private ClaimsPrincipal? ValidateTokenInternal(string token, bool validateLifetime)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_options.Secret);

        try
        {
            var principal = handler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = validateLifetime,       // 按需控制
                ValidIssuer = _options.Issuer,
                ValidAudience = _options.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero
            }, out _);
            return principal;
        }
        catch
        {
            return null;
        }
    }

    // =========================
    // 刷新 Token
    // =========================
    public JwtTokenResult RefreshToken(string refreshToken)
    {
        // 验证 refresh token
        var principal = ValidateTokenInternal(refreshToken, validateLifetime: true);
        if (principal == null)
            throw new Exception("RefreshToken无效");

        // 检查是否标记为 refresh 类型
        if (principal.FindFirst("token_type")?.Value != "refresh")
            throw new Exception("令牌类型不正确");

        var userIdStr = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!Guid.TryParse(userIdStr, out var userId))
            throw new Exception("RefreshToken 中用户标识无效");

        var userName = principal.FindFirst(ClaimTypes.Name)?.Value ?? "";

        // 签发新的 access + refresh（实现 refresh rotation 可选）
        return CreateToken(userId, userName);
    }
}