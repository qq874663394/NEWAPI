using Domain.Interface.Services.Authentication;
using Domain.Model.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Repositories.Authentication.Security
{
    public class JwtTokenService : ITokenService
    {
        private readonly JwtOptions _options;

        public JwtTokenService(
            IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }

        public string CreateToken(Guid userId)
        {
            var key =
                Encoding.UTF8.GetBytes(
                    _options.SecretKey);

            var claims = new List<Claim>
            {
                new Claim(
                    "userId",
                    userId.ToString()),

                new Claim(
                    JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString())
            };

            var token =
                new JwtSecurityToken(
                    issuer: _options.Issuer,
                    audience: _options.Audience,
                    claims: claims,
                    expires:
                        DateTime.UtcNow.AddMinutes(
                            _options.AccessTokenExpiration),
                    signingCredentials:
                        new SigningCredentials(
                            new SymmetricSecurityKey(key),
                            SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }

        public JwtToken? ValidateToken(string token)
        {
            var key =
                Encoding.UTF8.GetBytes(
                    _options.SecretKey);

            try
            {
                var handler =
                    new JwtSecurityTokenHandler();

                handler.ValidateToken(
                    token,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,

                        ValidIssuer =
                            _options.Issuer,

                        ValidAudience =
                            _options.Audience,

                        IssuerSigningKey =
                            new SymmetricSecurityKey(key),

                        ClockSkew = TimeSpan.Zero
                    },
                    out var validatedToken);

                var jwt =
                    (JwtSecurityToken)validatedToken;

                return new JwtToken
                {
                    UserId = Guid.Parse(
                        jwt.Claims.First(
                            x => x.Type == "userId").Value),

                    IsExpired = false
                };
            }
            catch (SecurityTokenExpiredException)
            {
                try
                {
                    var jwt =
                        new JwtSecurityTokenHandler()
                        .ReadJwtToken(token);

                    return new JwtToken
                    {
                        UserId = Guid.Parse(
                            jwt.Claims.First(
                                x => x.Type == "userId").Value),

                        IsExpired = true
                    };
                }
                catch
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public string RefreshToken(string token)
        {
            var jwt = ValidateToken(token);

            if (jwt == null)
            {
                throw new Exception("Token无效");
            }

            return CreateToken(jwt.UserId);
        }
    }
}
