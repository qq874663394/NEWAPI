namespace Application.Auth
{
    public class JwtTokenResult
    {
        public string AccessToken { get; set; } = default!;
        public string RefreshToken { get; set; } = default!;
        public DateTime ExpireAt { get; set; }
    }
}
