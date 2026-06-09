using Domain.Interface.IServices.Authentication;
using Domain.Model.Authentication;
namespace Infrastructure.Authentication
{
    public class AuthService : Domain.Interface.IServices.Authentication.IAuthService
    {
        private readonly AuthenticationProviderRegistry _registry;
        private readonly ITokenService _tokenService;

        public AuthService(
            AuthenticationProviderRegistry registry,
            ITokenService tokenService)
        {
            _registry = registry;
            _tokenService = tokenService;
        }

        public async Task<LoginResult> LoginAsync(AuthenticationRequest request)
        {
            var provider = _registry.Get(request.AuthType);

            var user = await provider.AuthenticateAsync(request);

            if (user == null)
                throw new Exception("登录失败");

            var token = _tokenService.CreateToken(user.Code);

            return new LoginResult
            {
                Token = token,
                UserCode = user.Code,
                UserName = user.Name ?? "",
                FullName = user.FullName
            };
        }
        public string RefreshToken(string token)
        {
            return _tokenService.RefreshToken(token);
        }

        public JwtToken? ValidateToken(string token)
        {
            return _tokenService.ValidateToken(token);
        }
    }
}
