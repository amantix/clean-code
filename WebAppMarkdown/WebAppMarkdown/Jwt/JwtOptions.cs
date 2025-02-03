namespace WebAppMarkdown.Jwt;

public class JwtOptions
{
    public int ExpiresInHours { get; set; } = 1;

    public string Secret { get; set; } = "your-256-bit-secret-key-hereyour-256-bit-secret-key-hereyour-256-bit-secret-key-hereyour-256-bit-secret-key-here";
}