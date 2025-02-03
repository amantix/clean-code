namespace API.Requests;

public class UserRequest
{
    public required string Login { get; set; }
    public required string Password { get; set; }
}