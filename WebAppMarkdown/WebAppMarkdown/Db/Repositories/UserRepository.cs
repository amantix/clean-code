using Microsoft.EntityFrameworkCore;
using WebAppMarkdown.Core.Models;
using WebAppMarkdown.Db.Interfaces;
using WebAppMarkdown.Db.Models;

namespace WebAppMarkdown.Db.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserEntity> GetUserByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<UserEntity> GetUserByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }
    
    public async Task<UserEntity> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task AddUserAsync(User user)
    {
        UserEntity userEntity = new UserEntity()
        {
            Email = user.Email,
            Id = user.Id,
            PasswordHash = user.PasswordHash,
            Username = user.Username
        };
        await _context.Users.AddAsync(userEntity);
        await _context.SaveChangesAsync();
    }
}