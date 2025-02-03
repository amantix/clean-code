using System.Security.Claims;
using WebAppMarkdown.Core.Models;
using WebAppMarkdown.Db.Repositories;
using WebAppMarkdown.Jwt;

namespace WebAppMarkdown.Services;

public class UsersService
{
    private readonly PasswordHasher _passwordHasher;
    private readonly UserRepository _usersRepository;
    private readonly JwtProvider _jwtProvider;

    public UsersService(
        UserRepository usersRepository,
        PasswordHasher passwordHasher,
        JwtProvider jwtProvider)
    {
        _passwordHasher = passwordHasher;
        _usersRepository = usersRepository;
        _jwtProvider = jwtProvider;
    }

    public async Task Register(string email, string username, string password)
    {
        var user = User.Create(Guid.NewGuid(), username, email, _passwordHasher.GenerateTokenSHA(password));
        await _usersRepository.AddUserAsync(user);
    }

    public async Task<string> Login(string email, string password)
    {
        var userEntity = await _usersRepository.GetUserByEmailAsync(email);
        
        var user = Core.Models.User.Create(userEntity.Id, userEntity.Username, userEntity.Email, userEntity.PasswordHash);

        var result = _passwordHasher.Verify(password, user.PasswordHash);

        if (result == false)
        {
            throw new Exception("Failed to login");
        }

        var token = _jwtProvider.GenerateToken(user);

        return token;
    }
    
    public async Task<Guid?>GetUserIdFromToken(ClaimsPrincipal user)
    {
        var userIdClaim = user.Claims.FirstOrDefault(x => x.Type == "userId")?.Value;

        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            return null; // Возвращаем null, если нет userId или он некорректен
        }

        return userId; // Возвращаем корректный userId
    }
    
}