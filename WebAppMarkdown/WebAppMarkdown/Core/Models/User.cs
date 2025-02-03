namespace WebAppMarkdown.Core.Models;

public class User
{
    public Guid Id { get; set; }
    
    public string Username { get; set; }
    
    public string Email { get; set; }

    public string PasswordHash { get; set; }

    public List<Document> Documents { get; set; } = [];
    

    public static User Create(Guid id, string username, string email, string passwordHash)
    {
        return new User
        {
            Id = id,
            Username = username,
            Email = email,
            PasswordHash = passwordHash
        };
    }

}