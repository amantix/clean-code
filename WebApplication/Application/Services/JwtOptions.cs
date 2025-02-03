namespace Application.Services;

public class JwtOptions
{
    public string UserId { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpireMinutes { get; set; }
    public string Secret { get; set; }
}