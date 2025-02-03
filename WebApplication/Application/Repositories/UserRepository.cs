using Application.Abstract.Repositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Entities;

namespace Application.Repositories;

public class UserRepository(AppDbContext context): IUserRepository
{
    public async Task<bool> CreateUser(User user)
    {
        var userEntity = new UserEntity
        {
            UserEmail = user.Email,
            UserName = user.Name,
            PasswordHash = user.PasswordHash,
        };
        try
        {
            await context.Users.AddAsync(userEntity);
            await context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public async Task<bool> UpdateUser(User user)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> DeleteUserById(Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        var user = await context.Users
            .AsNoTracking()
            .Where(x => x.UserEmail == email)
            .FirstOrDefaultAsync();
        if (user == null) return null;
        return new User
        {
            Id = user.UserId,
            Email = user.UserEmail,
            Name = user.UserName,
            PasswordHash = user.PasswordHash,
        };
    }

    public async Task<User?> GetUserById(Guid id)
    {
        var userEntity = await context.Users
            .AsNoTracking()
            .Where(x => x.UserId == id)
            .FirstOrDefaultAsync();
        if (userEntity == null) return null;
        return new User
        {
            Id = userEntity.UserId,
            Email = userEntity.UserEmail,
            Name = userEntity.UserName,
            PasswordHash = userEntity.PasswordHash,
        };
    }
}