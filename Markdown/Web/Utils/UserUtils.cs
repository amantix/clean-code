using Services.Abstracts;
using Services.Schemas;
using System.Security.Claims;

public class UserUtils
{
    private readonly IUserService _userService;

    public UserUtils(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<bool> IsValidUser(ClaimsPrincipal user)
    {
        var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
        Console.WriteLine("User: " + userIdClaim);
        if (!int.TryParse(userIdClaim, out var userId))
        {
            return false;
        }

        var isExists = await _userService.IsUserExistAsync(userId);

        if (!isExists)
        {
            return false;
        }

        return true;
    }

    public async Task<bool> IsUserAuthor(int userId, DocumentDTO document)
    {
        if (document.UserID != userId && !document.IsPublic)
        {
            return false;
        }

        return true;
    }
}
