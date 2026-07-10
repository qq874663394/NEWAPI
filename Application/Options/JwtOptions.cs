namespace Application.Options
{
    public class JwtOptions
    {
        public string Issuer { get; set; } = "AuthApplication";

        public string Audience { get; set; } = "AuthApplication";

        public string Secret { get; set; } = "THIS_IS_DEMO_SECRET_KEY_CHANGE_IT";

        public int ExpireMinutes { get; set; } = 120;
    }
}
